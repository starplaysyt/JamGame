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
            SDL_ttf.TTF_Init();
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG);

            Console.WriteLine(SDL_GetError());
        }
    }
}

