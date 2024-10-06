namespace JackalWebHost2.Exceptions;

public class AllLobbyMembersMustHaveTeamException : BusinessException
{
    public override string ErrorMessage => "Все участники комнаты должны выбрать команду";

    public override string ErrorCode => ErrorCodes.AllLobbyMembersMustHaveTeam;
}