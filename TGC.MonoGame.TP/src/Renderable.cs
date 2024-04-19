using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.MonoGame.TP {
    class Renderable {
        
        private Model _model;
        private Shader _shader;

        public Renderable(Shader shader, Model model){
            
            _shader = shader;
            _model = model;

            _shader.AssociateShaderTo(model);

        }

        public void Draw(Matrix world, Camera camera){

            _shader.UseCamera(camera);
            _shader.ApplyEffects();

            foreach (var mesh in _model.Meshes)
            {
                Matrix modelWorld = mesh.ParentBone.Transform * world;
                _shader.UseWorld(modelWorld);
                mesh.Draw();
            }

        }
        

    }

}