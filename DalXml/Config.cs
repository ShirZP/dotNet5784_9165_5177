namespace Dal;
internal static class Config
{
    internal static string s_data_config_xml = "data-config";

    internal static DateTime? ProjectStartDate { get => XMLTools.GetProjectDate(s_data_config_xml, "ProjectStartDate");}
    internal static DateTime? ProjectEndDate { get => XMLTools.GetProjectDate(s_data_config_xml, "ProjectEndDate");}
    internal static DO.ProjectStatus ProjectStatus { get => XMLTools.GetProjectStatus(s_data_config_xml, "ProjectStatus");}
    internal static DateTime Clock { get => XMLTools.GetClock(s_data_config_xml, "Clock");}

    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }

}
