using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

/// <summary>
/// Represents the people who contributed to the module's development.
/// </summary>
public class Contributors
{
    /// <summary>
    /// People who contributed audio for the mod.
    /// </summary>
    public List<string> Audio { get; set; }
    /// <summary>
    /// People who developed (programmed) the mod.
    /// </summary>
    public List<string> Developer { get; set; }
    /// <summary>
    /// People who contributed the original idea for the mod.
    /// </summary>
    public List<string> Idea { get; set; }
    /// <summary>
    /// People who are maintaining the mod. 
    /// </summary>
    public List<string> Maintainer { get; set; }
    /// <summary>
    /// People who contributed the manual.
    /// </summary>
    public List<string> Manual { get; set; }
    /// <summary>
    /// People who contributed graphics for the manual.
    /// </summary>

    [JsonProperty("Manual graphics")]
    public List<string> Manualgraphics { get; set; }
    /// <summary>
    /// People who contributed 3D models for the mod.
    /// </summary>
    public List<string> Modeling { get; set; }

    /// <summary>
    /// People who added Twitch Plays support. 
    /// </summary>
    [JsonProperty("Twitch Plays")]
    public List<string> TwitchPlays { get; set; }
    /// <returns>All of the people who contributed to the module in one list.</returns>
    public List<string> GetAllContributors()
    {
        return new[] { Audio, Developer, Idea, Maintainer, Manual, Manualgraphics, Modeling, TwitchPlays }
                        .Where(x => x != null)
                        .SelectMany(x => x).ToList();
    }
}

public class KtaneModule
{
    /// <summary>
    /// The string listed on the repository page as the author(s) of the module. Use the "Contributors" property if possible.
    /// </summary>
    public string Author { get; set; }
    /// <summary>
    /// Represents whether a module requires input or displays new information after each solve.<br></br>
    /// <i>SemiBoss, FullBoss, or null</i>
    /// </summary>

    public string BossStatus { get; set; }
    /// <summary>
    /// Specifies if the module or widget has any known issues.<br></br>
    /// <i>Compatible, Problematic, or Unplayable</i>
    /// </summary>
    public string Compatibility { get; set; }
    /// <summary>
    /// If the module is problematic or unplayable, explains the issue.
    /// </summary>
    public string CompatibilityExplanation { get; set; }
    /// <summary>
    /// Stores the contributors listed for the module.
    /// </summary>
    public Contributors Contributors { get; set; }
    /// <summary>
    /// An optional alternative defuser difficulty descriptor.
    /// </summary>
    public string CustomDefuserDifficulty { get; set; }
    /// <summary>
    /// An optional alternative expert difficulty descriptor.
    /// </summary>
    public string CustomExpertDifficulty { get; set; }
    /// <summary>
    /// Stores if Demand-Based Mod Loading should not be used to load the module.
    /// </summary>
    public bool DBMLIgnored { get; set; }
    /// <summary>
    /// An approximate difficulty rating for the defuser.<br></br>
    /// <i>VeryEasy, Easy, Medium, Hard, or VeryHard</i>
    /// </summary>
    public string DefuserDifficulty { get; set; }
    /// <summary>
    /// A concise description of what sets this module or widget apart from others which includes tags identifying the module.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// A <b>deprecated</b> tag used by only a few modules representing the their display name. <br></br>
    /// When referencing the display name of a module, <b>always</b> reference the "Name" property instead.
    /// </summary>
    public string DisplayName { get; set; }
    /// <summary>
    /// An approximate difficulty rating for the expert.
    /// <i>VeryEasy, Easy, Medium, Hard, or VeryHard</i>
    /// </summary>
    public string ExpertDifficulty { get; set; }
    /// <summary>
    /// Only used by boss modules.<br></br>
    /// A list of module names or special tags indicating what other modules the module ignores.
    /// </summary>
    public List<string> Ignore { get; set; }
    /// <summary>
    /// Only used by boss modules.<br></br>
    /// The complete list of module names which the module ignores.
    /// </summary>
    public List<string> IgnoreProcessed { get; set; }
    /// <summary>
    /// Specifies how the module's content can be reused and republished.<br></br>
    /// If this field is null, the presence of a source code URL indicates whether the module can be republished.<br></br>
    /// <i>Republishable, OpenSourceClone, or null</i>
    /// </summary>
    public string License { get; set; }
    /// <summary>
    /// The ID that mission makers need for this module. This is the same as the ModuleType property on the KMBombModule component.
    /// </summary>
    public string ModuleID { get; set; }
    /// <summary>
    /// Indicates how Mystery Module will treat the module.<br></br>
    /// <i>MustNotBeHidden, MustNotBeKey, MustNotBeHiddenOrKey, RequiresAutoSolver, or null</i>
    /// </summary>
    public string MysteryModule { get; set; }
    /// <summary>
    /// The display name of the module or widget.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Numerical IDs of Steam Workshop items containing old versions of this mod that have since been reuploaded.
    /// </summary>
    public List<string> ObsoleteSteamIDs { get; set; }
    /// <summary>
    /// Indicates where the module came from.<br></br>
    /// <i>Vanilla or Mods</i>
    /// </summary>
    public string Origin { get; set; }
    /// <summary>
    /// The date the module was published in YYYY-MM-DD format.
    /// </summary>
    public string Published { get; set; }
    /// <summary>
    /// A comma-separated string representing various traits about the module.<br></br>
    /// <i>SolvesAtEnd, NeedsOtherSolves, SolvesBeforeSome, SolvesWithOthers, WillSolveSuddenly, PseudoNeedy, TimeDependent, NeedsImmediateAttention, or InstantDeath</i>
    /// </summary>
    public string Quirks { get; set; }
    /// <summary>
    /// Whether or not the module alters its rules under the Rule Seed Modifier.<br></br>
    /// <i>Supported or null</i>
    /// </summary>
    public string RuleSeedSupport { get; set; }
    /// <summary>
    /// A series of strings representing which manuals are available for a module.<br></br>
    /// (HTML vs. PDF variants, as well as any alternate manuals.)
    /// </summary>
    public List<string> Sheets { get; set; }
    /// <summary>
    /// The name of the module or widget in all-caps, without spaces, and without initial “The”.<br></br>
    /// Used to sort the module on the repository alphabetically.
    /// </summary>
    public string SortKey { get; set; }
    /// <summary>
    /// A link to the source code of the module or widget, usually a link to a GitHub repository.
    /// </summary>
    public string SourceUrl { get; set; }
    /// <summary>
    /// Represents the state of Souvenir support for the module.
    /// </summary>
    public Souvenir Souvenir { get; set; }
    /// <summary>
    /// The numerical ID of the Steam Workshop item.
    /// </summary>
    public string SteamID { get; set; }
    /// <summary>
    /// A symbol for the Periodic Table of Modules. Only the first letter will be capitalized.
    /// </summary>
    public string Symbol { get; set; }
    /// <summary>
    /// Represents the scoring used to calculate the amount of time gained upon a solve when in Time Mode.
    /// </summary>
    public TimeMode TimeMode { get; set; }
    /// <summary>
    /// If the module is a translation of another, the display name of the original module.
    /// </summary>
    public string TranslationOf { get; set; }
    /// <summary>
    /// The tutorial videos available for the module.
    /// </summary>
    public List<TutorialVideo> TutorialVideos { get; set; }
    /// <summary>
    /// Represents the score of the module in Twitch Plays.
    /// </summary>
    public TwitchPlays TwitchPlays { get; set; }
    /// <summary>
    /// Represents what kind of item is specified by the JSON.<br></br>
    /// <i>Regular, Needy, Widget, or Holdable</i>
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// The x-position of the module's icon on the icon sheet, zero-indexed from left to right.
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// The y-position of the module's icon on the icon sheet, zero-indexed from the top down.
    /// </summary>
    public int Y { get; set; }

}
/// <summary>
/// Wrapper class representing the JSON available at <see href="https://ktane.timwi.de/json/raw"></see>.
/// </summary>
public class Root
{
    /// <summary>
    /// A list containing every module on the repo.
    /// </summary>
    public List<KtaneModule> KtaneModules { get; set; }
}
/// <summary>
/// Represents the state of Souvenir support for a module.
/// </summary>
public class Souvenir
{
    /// <summary>
    /// If the module is considered for Souvenir support, explain what question(s) Souvenir could ask.
    /// </summary>    
    public string Explanation { get; set; }
    /// <summary>
    /// Represents whether or not the module is a candidate for Souvenir support.<br></br>
    /// <i>NotACandidate, Considered, or Supported</i>
    /// </summary>
    public string Status { get; set; }
}

public class TimeMode
{
    /// <summary>
    /// Represents the numerical base score of the module.
    /// </summary>
    public double Score { get; set; }
    /// <summary>
    /// Represents what determines the Time Mode score of the module.<br></br>
    /// <i>Assigned, Unassigned, Community, or TwitchPlays</i>
    /// </summary>
    public string Origin { get; set; }
    /// <summary>
    /// If the module is a boss module, represents the score awarded per other module on the bomb.
    /// </summary>
    public double? ScorePerModule { get; set; }
}
/// <summary>
/// Represents a tutorial video listed under a module.
/// </summary>
public class TutorialVideo
{
    /// <summary>
    /// If there are multiple tutorials available for a module in the same language, serves to distinguish them.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// The language the tutorial is given in.
    /// </summary>
    public string Language { get; set; }
    /// <summary>
    /// A link to the tutorial, usually a YouTube link.
    /// </summary>
    public string Url { get; set; }
}
/// <summary>
/// Represents the score of the module in Twitch Plays.
/// </summary>
public class TwitchPlays
{
    /// <summary>
    /// The base score of the module in Twitch Plays. <br></br>
    /// If the module's scoring is dependent on the number of modules present, assume 10 modules on the bomb.
    /// </summary>
    public double Score { get; set; }
    /// <summary>
    /// Describes the particular way the module awards points.
    /// </summary>
    public string ScoreStringDescription { get; set; }
}

