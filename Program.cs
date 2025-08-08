using System;
using JamGame.Graphics;
using SDL2;
using static SDL2.SDL;

namespace JamGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SDL_Init(SDL_INIT_EVERYTHING);

            bool isRunning = true;
            
            WindowContext context = new WindowContext();

            while (isRunning)
            {
                while (context.GetSDLEvent(out var evt) != 0)
                {
                    //Event handling block
                }
                
                //Rendering block
            }
        }
    }
}

