#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
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

#endregion

using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

namespace SetupTv
{
  public partial class WizardForm : SetupControls.MPForm
  {
    internal class SectionHolder
    {
      public SectionSettings Section;
      public string Topic;
      public string Information;
      public string Expression;

      public SectionHolder(SectionSettings section, string topic, string information, string expression)
      {
        Section = section;
        Topic = topic;
        Information = information;
        Expression = expression;
      }
    }


    public WizardForm()
    {
      InitializeComponent();
    }


    //
    // Private members
    //
    public static ArrayList WizardPages
    {
      get { return wizardPages; }
    }

    private static readonly ArrayList wizardPages = new ArrayList();

    public static WizardForm Form
    {
      get { return wizardForm; }
    }

    private static WizardForm wizardForm;

    private string wizardCaption = String.Empty;

    private int visiblePageIndex = -1;

    public void AddSection(SectionSettings settings, string topic, string information)
    {
      AddSection(settings, topic, information, String.Empty);
    }

    public void AddSection(SectionSettings settings, string topic, string information, string expression)
    {
      wizardPages.Add(new SectionHolder(settings, topic, information, expression));
    }

    public void DisableBack(bool disabled)
    {
      backButton.Enabled = !disabled;
    }

    public void DisableNext(bool disabled)
    {
      nextButton.Enabled = !disabled;
    }

    public WizardForm(string sectionConfiguration)
    {
      wizardForm = this;
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // Set caption
      //
      wizardCaption = "MediaPortal Settings Wizard";

      //
      // Check if we got a sections file to read from, or if we should specify
      // the default sections
      //
      if (sectionConfiguration != String.Empty && System.IO.File.Exists(sectionConfiguration))
      {
        LoadSections(sectionConfiguration);
      }
    }

    /// <summary>
    /// Loads, parses and creates the defined sections in the section xml.
    /// </summary>
    /// <param name="xmlFile"></param>
    private void LoadSections(string xmlFile)
    {
      XmlDocument document = new XmlDocument();

      try
      {
        //
        // Load the xml document
        //
        document.Load(xmlFile);

        XmlElement rootElement = document.DocumentElement;

        //
        // Make sure we're loading a wizard file
        //
        if (rootElement != null && rootElement.Name.Equals("wizard"))
        {
          //
          // Fetch wizard settings
          //
          XmlNode wizardTopicNode = rootElement.SelectSingleNode("/wizard/caption");
          if (wizardTopicNode != null)
          {
            wizardCaption = wizardTopicNode.InnerText;
          }

          //
          // Fetch sections
          //
          XmlNodeList nodeList = rootElement.SelectNodes("/wizard/sections/section");

          if (nodeList != null)
            foreach (XmlNode node in nodeList)
            {
              //
              // Fetch section information
              //
              XmlNode nameNode = node.SelectSingleNode("name");
              XmlNode topicNode = node.SelectSingleNode("topic");
              XmlNode infoNode = node.SelectSingleNode("information");
              XmlNode dependencyNode = node.SelectSingleNode("dependency");

              if (nameNode != null && nameNode.InnerText.Length > 0)
              {
                //
                // Allocate new wizard page
                //
                SectionSettings section = CreateSection(nameNode.InnerText);

                if (section != null)
                {
                  //
                  // Load wizard specific settings
                  //
                  section.LoadWizardSettings(node);

                  //
                  // Add the section to the sections list
                  //
                  if (dependencyNode == null)
                  {
                    AddSection(section, topicNode != null ? topicNode.InnerText : String.Empty,
                               infoNode != null ? infoNode.InnerText : String.Empty);
                  }
                  else
                  {
                    AddSection(section, topicNode != null ? topicNode.InnerText : String.Empty,
                               infoNode != null ? infoNode.InnerText : String.Empty, dependencyNode.InnerText);
                  }
                }
              }
            }
        }
      }
      catch (Exception e)
      {
        System.Diagnostics.Debug.WriteLine(e.Message);
      }
    }

    /// <summary>
    /// Creates a section class from the specified name
    /// </summary>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    private static SectionSettings CreateSection(string sectionName)
    {
      Type sectionType = Type.GetType("MediaPortal.Configuration.Sections." + sectionName);

      if (sectionType != null)
      {
        //
        // Create the instance of the section settings class, pass the section name as argument
        // to the constructor. We do this to be able to use the same name on <name> as in the <dependency> tag.
        //
        SectionSettings section = (SectionSettings)Activator.CreateInstance(sectionType, new object[] {sectionName});
        return section;
      }

      //
      // Section was not found
      //
      return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WizardForm_Load(object sender, EventArgs e)
    {
      //
      // Load settings
      //
      LoadSectionSettings();

      //
      // Load first page
      //
      ShowNextPage();
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowNextPage()
    {
      //
      // Make sure we have something to show
      //
      while (true)
      {
        if (visiblePageIndex + 1 < wizardPages.Count)
        {
          //
          // Move to next index, the index  that will be shown
          //
          visiblePageIndex++;

          //
          // Activate section
          //
          SectionHolder holder = wizardPages[visiblePageIndex] as SectionHolder;

          if (holder != null)
          {
            //
            // Evaluate if this section should be shown at all
            //
            if (EvaluateExpression(holder.Expression))
            {
              ActivateSection(holder.Section);

              //
              // Set topic and information
              //
              SetTopic(holder.Topic);
              SetInformation(holder.Information);

              break;
            }
          }
        }
        else
        {
          //
          // No more sections to show
          //
          break;
        }
      }

      //
      // Update control status
      //
      UpdateControlStatus();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    private static bool EvaluateExpression(string expression)
    {
      if (expression.Length > 0)
      {
        int dividerPosition = expression.IndexOf(".");

        string section = expression.Substring(0, dividerPosition);
        string property = expression.Substring(dividerPosition + 1);

        //
        // Fetch section
        //
        foreach (SectionHolder holder in wizardPages)
        {
          string sectionName = holder.Section.Text.ToLowerInvariant();

          if (sectionName.Equals(section.ToLowerInvariant()))
          {
            //
            // Return property
            //
            return (bool)holder.Section.GetSetting(property);
          }
        }

        return false;
      }

      return true;
    }

    private void SetTopic(string topic)
    {
      topicLabel.Text = topic;
    }

    private void SetInformation(string information)
    {
      infoLabel.Text = information;
    }

    private void ShowPreviousPage()
    {
      //
      // Make sure we have something to show
      //
      while (true)
      {
        if (visiblePageIndex - 1 >= 0)
        {
          //
          // Move to previous index
          //
          visiblePageIndex--;

          //
          // Activate section
          //
          SectionHolder holder = wizardPages[visiblePageIndex] as SectionHolder;

          if (holder != null)
          {
            //
            // Evaluate if this section should be shown at all
            //
            if (EvaluateExpression(holder.Expression))
            {
              ActivateSection(holder.Section);

              //
              // Set topic and information
              //
              SetTopic(holder.Topic);
              SetInformation(holder.Information);

              break;
            }
          }
        }
        else
        {
          //
          // No more pages to show
          //
          break;
        }
      }

      //
      // Update control status
      //
      UpdateControlStatus();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="section"></param>
    private void ActivateSection(SectionSettings section)
    {
      section.Dock = DockStyle.Fill;

      section.OnSectionActivated();

      holderPanel.Controls.Clear();
      holderPanel.Controls.Add(section);
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
      SectionHolder holder = wizardPages[visiblePageIndex] as SectionHolder;
      if (holder != null) holder.Section.SaveSettings();
      if (visiblePageIndex == wizardPages.Count - 1)
      {
        //
        // This was the last page, finish off the wizard
        //
        SaveSectionSettings();

        Close();
      }
      else
      {
        //
        // Show the next page of the wizard
        //
        ShowNextPage();
      }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void backButton_Click(object sender, EventArgs e)
    {
      ShowPreviousPage();
    }

    private void UpdateControlStatus()
    {
      backButton.Enabled = visiblePageIndex > 0;
      nextButton.Enabled = true;

      nextButton.Text = visiblePageIndex == wizardPages.Count - 1 ? "&Finish" : "&Next >";

      //
      // Set caption
      //
      Text = String.Format("{0} [{1}/{2}]", wizardCaption, visiblePageIndex + 1, wizardPages.Count);
    }

    /// <summary>
    /// 
    /// </summary>
    private static void LoadSectionSettings()
    {
      foreach (SectionHolder holder in wizardPages)
      {
        holder.Section.LoadSettings();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private static void SaveSectionSettings()
    {
      foreach (SectionHolder holder in wizardPages)
      {
        holder.Section.SaveSettings();
      }
    }
  }
}