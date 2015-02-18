using System;
using Home3d.Model;

namespace Home3d
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var newScene = new Scene())
            {
                newScene.Run(30.0, 0.0);
            }
        }

    }
}