using System;

namespace ServerLib
{
    public class DbService
	{
        public UserContext GameContext { get; }
        public AuthContext AuthContext { get; }
        public AuthRepository Auth { get; }
        public GameRepository Game { get; }

        public DbService(UserContext gameContext, AuthContext authContext)
		{
            GameContext = gameContext;
            AuthContext = authContext;
            Auth = new AuthRepository(authContext);
            Game = new GameRepository(gameContext);
		}
	}
}


