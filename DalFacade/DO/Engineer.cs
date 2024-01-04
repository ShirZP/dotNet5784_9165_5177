namespace DO;
/// <summary>
/// An engineer entity represents an engineer with all its properties
/// </summary>
/// <param name="ID">The engineer's unique personal identity card (as in a national identity card)</param>
/// <param name="FullName">Engineer's first and last name</param>
/// <param name="Email">The email address of the engineer</param>
/// <param name="Level">The level of expertise of the engineer</param>
/// <param name="Cost">How much per hour does the engineer get</param>
public record Engineer
(
    int ID,
    string? FullName,
    string? Email,
    DO.EngineerExperience? Level,
    double? Cost
)
{
    public Engineer() : this(0, "", "", null, 0) { } //empty ctor for stage 3

}
