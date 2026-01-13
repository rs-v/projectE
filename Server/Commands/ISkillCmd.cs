namespace Server.Commands
{
    public interface ISkillCmd : ICommand
    {
        string SkillName { get; }
        string SkillDescription { get; }
        string[] SkillAliases { get; }
        string[] SkillParameters { get; }
        bool IsEnabled { get; set; }


    }
}
