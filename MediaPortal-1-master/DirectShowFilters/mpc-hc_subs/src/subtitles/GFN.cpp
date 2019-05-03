/*
 * (C) 2003-2006 Gabest
 * (C) 2006-2013 see Authors.txt
 *
 * This file is part of MPC-HC.
 *
 * MPC-HC is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or
 * (at your option) any later version.
 *
 * MPC-HC is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

#include "stdafx.h"
#include <io.h>
#include <atlrx.h>
#include "TextFile.h"
#include "GFN.h"

LPCTSTR exttypestr[] = {
    _T("srt"), _T("sub"), _T("smi"), _T("psb"),
    _T("ssa"), _T("ass"), _T("idx"), _T("usf"),
    _T("xss"), _T("txt"), _T("rt"), _T("sup")
};

static LPCTSTR separators = _T(".\\-_");
static LPCTSTR extListVid = _T("(avi)|(mkv)|(mp4)|((m2)?ts)");

static int SubFileCompare(const void* elem1, const void* elem2)
{
    return ((SubFile*)elem1)->fn.CompareNoCase(((SubFile*)elem2)->fn);
}

void GetSubFileNames(CString fn, const CAtlArray<CString>& paths, CAtlArray<SubFile>& ret)
{
    ret.RemoveAll();

    fn.Replace('\\', '/');

    bool fWeb = false;
    {
        //int i = fn.Find(_T("://"));
        int i = fn.Find(_T("http://"));
        if (i > 0) {
            fn = _T("http") + fn.Mid(i);
            fWeb = true;
        }
    }

    int l  = fn.ReverseFind('/') + 1;
    int l2 = fn.ReverseFind('.');
    if (l2 < l) { // no extension, read to the end
        l2 = fn.GetLength();
    }

    CString orgpath = fn.Left(l);
    CString title = fn.Mid(l, l2 - l);
    int titleLength = title.GetLength();
    //CString filename = title + _T(".nooneexpectsthespanishinquisition");

    if (!fWeb) {
        WIN32_FIND_DATA wfd;
        HANDLE hFile;

        CString extListSub, regExpSub, regExpVid;
        for (int i = 0; i < _countof(exttypestr); i++) {
            extListSub.AppendFormat(_T("(%s)"), exttypestr[i]);
            if (i < _countof(exttypestr) - 1) {
                extListSub.AppendChar(_T('|'));
            }
        }
        regExpSub.Format(_T("([%s]+.+)?\\.(%s)$"), separators, extListSub);
        regExpVid.Format(_T(".+\\.(%s)$"), extListVid);

        CAtlRegExp<CAtlRECharTraits> reSub, reVid;
        CAtlREMatchContext<CAtlRECharTraits> mc;
        ENSURE(REPARSE_ERROR_OK == reSub.Parse(regExpSub, FALSE));
        ENSURE(REPARSE_ERROR_OK == reVid.Parse(regExpVid, FALSE));

        for (size_t k = 0; k < paths.GetCount(); k++) {
            CString path = paths[k];
            //path.Replace('\\', '/');

            l = path.GetLength();
            if (l > 0 && path[l - 1] != '/') {
                path += '/';
            }

            if (path.Find(':') == -1 && path.Find(_T("\\\\")) != 0) {
                path = orgpath + path;
            }

            path.Replace(_T("/./"), _T("/"));
            path.Replace('/', '\\');

            CAtlList<CString> subs, vids;

            if ((hFile = FindFirstFile(path + title + _T("*"), &wfd)) != INVALID_HANDLE_VALUE) {
                do {
                    CString fn = path + wfd.cFileName;
                    if (reSub.Match(&wfd.cFileName[titleLength], &mc)) {
                        subs.AddTail(fn);
                    } else if (reVid.Match(&wfd.cFileName[titleLength], &mc)) {
                        // Convert to lower-case and cut the extension for easier matching
                        vids.AddTail(fn.Left(fn.ReverseFind(_T('.'))).MakeLower());
                    }
                } while (FindNextFile(hFile, &wfd));

                FindClose(hFile);
            }

            POSITION posSub = subs.GetHeadPosition();
            while (posSub) {
                CString& fn = subs.GetNext(posSub);
                CString fnlower = fn;
                fnlower.MakeLower();

                // Check if there is an exact match for another video file
                bool bMatchAnotherVid = false;
                POSITION posVid = vids.GetHeadPosition();
                while (posVid) {
                    if (fnlower.Find(vids.GetNext(posVid)) == 0) {
                        bMatchAnotherVid = true;
                        break;
                    }
                }

                if (!bMatchAnotherVid) {
                    SubFile f;
                    f.fn = fn;
                    ret.Add(f);
                }
            }
        }
    } else if (l > 7) {
        CWebTextFile wtf; // :)
        if (wtf.Open(orgpath + title + _T(".wse"))) {
            CString fn2;
            while (wtf.ReadString(fn2) && fn2.Find(_T("://")) >= 0) {
                SubFile f;
                f.fn = fn2;
                ret.Add(f);
            }
        }
    }

    // sort files, this way the user can define the order (movie.00.English.srt, movie.01.Hungarian.srt, etc)

    qsort(ret.GetData(), ret.GetCount(), sizeof(SubFile), SubFileCompare);
}
