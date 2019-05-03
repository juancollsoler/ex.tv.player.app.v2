// Copyright (C) 2016 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

// ========================================================================
// The code in this file is derived from the 'HEVCESBrowser' project,
// a tool for analyzing HEVC(h265) bitstreams authored by 'virinext'.
// See https://github.com/virinext/hevcesbrowser
// and http://www.codeproject.com/Tips/896030/The-Structure-of-HEVC-Video
// Licensed under the GNU General Public License and 
// the Code Project Open License, http://www.codeproject.com/info/cpol10.aspx
// ========================================================================

#ifndef HEVC_NAL_DECODE
#define HEVC_NAL_DECODE

#include "Hevc.h"
#include "BitstreamReader.h"

#include <map>
#include <list>
#include <memory>

namespace HEVC
{
  class HevcNalDecode
  {
    public:
      NALUnitType processNALUnit(const uint8_t *pdata, std::size_t size, hevchdr& h);

    protected:
      NALUnitType processNALUnitHeader(BitstreamReader &bs);
      void Remove3Byte(uint8_t* dst, const uint8_t* src, int length);
      void processSPS(std::shared_ptr<SPS> psps, BitstreamReader &bs);
      ProfileTierLevel processProfileTierLevel(std::size_t max_sub_layers_minus1, BitstreamReader &bs);
      HrdParameters processHrdParameters(uint8_t commonInfPresentFlag, std::size_t maxNumSubLayersMinus1, BitstreamReader &bs);
      ShortTermRefPicSet processShortTermRefPicSet(std::size_t stRpsIdx, size_t num_short_term_ref_pic_sets, const std::vector<ShortTermRefPicSet> &refPicSets, std::shared_ptr<SPS> psps, BitstreamReader &bs);
      VuiParameters processVuiParameters(std::size_t sps_max_sub_layers_minus1, BitstreamReader &bs);
      ScalingListData processScalingListData(BitstreamReader &bs);
      SubLayerHrdParameters processSubLayerHrdParameters(uint8_t sub_pic_hrd_params_present_flag, std::size_t CpbCnt, BitstreamReader &bs);
  };
}

#endif

