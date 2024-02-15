using System.Collections;
using System.Linq;

namespace PL;


internal class ExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums = (Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}


internal class EngineerExperienceCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums = (Enum.GetValues(typeof(BO.EngineerExperience)).Cast<BO.EngineerExperience>().Where(level => level != BO.EngineerExperience.All) as IEnumerable<BO.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
