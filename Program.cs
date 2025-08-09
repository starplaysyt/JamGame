using System;
using JamGame.Core;
using JamGame.Game;
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
            
            LayerController lc = new LayerController();
            
            while (isRunning)
            {
                while (WindowContext.GetSDLEvent(out var evt) != 0)
                {
                    if(evt.type == SDL_EventType.SDL_QUIT)
                    {
                        isRunning = false;
                    }
                    lc.Update(ref evt);
                    //Event handling block
                }
                
                lc.UpdateByTick();

                WindowContext.Renderer.RendererColor = new Color(255, 0, 0);

                WindowContext.Renderer.DrawLine(10, 10, 100, 100);
                
                lc.DrawLayers();
                
                WindowContext.Renderer.RendererColor = new Color(255, 0, 255);
                WindowContext.Renderer.RenderComplete();

                //Rendering block

                //VSYNC controller
            }
        }
    }
}

