namespace Core.models.enums.errors
{
    public enum EventErrorEnum : uint
    {
        Success = 0,
        Login_ALREADY_LOGIN_WEB = 0x80000101,
        Login_USER_PASS_FAIL = 0x80000102,
        //07
        Login_LOGOUTING = 0x80000104,
        Login_TIME_OUT_1 = 0x80000105,
        Login_TIME_OUT_2 = 0x80000106,
        Login_BLOCK_ACCOUNT = 0x80000107,
        //13
        Login_SERVER_USER_FULL = 0x8000010E,
        Login_DB_BUFFER_FAIL = 0x80000114,
        Login_ID_PASS_INCORRECT = 0x80000117, //ID ou senha incorretos. (Usuário)
        Login_ID_PASS_INCORRECT2 = 0x80000118, //ID ou senha incorretos. (Senha)
        Login_DELETE_ACCOUNT = 0x80000119,
        Login_EMAIL_AUTH_ERROR = 0x80000120,
        Login_BLOCK_IP = 0x80000121,
        Login_EMAIL_ALERT2 = 0x80000122,
        //39
        Login_MIGRATION = 0x80000124,
        Login_NON_STRING = 0x80000125,
        Login_BLOCK_COUNTRY = 0x80000126, //Login bloqueado devido à região.
        Login_INVALID_ACCOUNT = 0x80000127, //Usuário ou senha incorreta.
        //41?
        //44 45 46 47
        Login_PC_BLOCK = 0x80000133, //Este computador está bloqueado.
        Login_INVENTORY_FAIL = 0x80001034,

        Login_BLOCK_INNER = 0x80100000,
        Login_BLOCK_OUTER = 0x80200000,
        Login_BLOCK_GAME = 0x80800000,


        Battle_No_Real_IP = 0x80001008, //No RealIP
        Battle_No_Ready_Team = 0x80001009, //I was not a 2 team
        Battle_First_MainLoad = 0x8000100A, //Loading (out of time)
        Battle_First_Hole = 0x8000100B, //Out by hole punching
        Battle_No_Enemy = 0x8000100C, //Praptor - Do not use
        Battle_Wait_Battle_Climax = 0x8000100F, //Praptor - Do not use
        Battle_No_Start_For_Under_NAT = 0x80001012, //Under the NAT that can not start the game
        Battle_PreStart_Battle_AlreadyEnd = 0x80001015, //The game is over when I am on board.
        Battle_Start_Battle_AlreadyEnd = 0x80001016, //The game is over when I am on board.


        VisitEvent_UserFail = 0x80001500,
        VisitEvent_NotEnough = 0x80001501,
        VisitEvent_AlreadyCheck = 0x80001502,
        VisitEvent_WrongVersion = 0x80001503,
        VisitEvent_Success = 0x80001504,
        VisitEvent_Unknown = 0x80001505
    }
}