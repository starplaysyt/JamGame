using System;
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
            
            IntPtr window = SDL_CreateWindow("testwindow", 
                SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED,
                200, 200, SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            while (isRunning)
            {
                while (SDL_PollEvent(out var evt) != 0)
                {
                    //Event handling block
                }
                
                //Rendering block
            }
        }
    }
}

