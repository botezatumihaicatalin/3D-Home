using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Home3d.Model
{
    /// <summary>
    /// An individual obj object which contains information about it's faces.
    /// </summary>
    public class ObjObject : IDisposable
    {
        public ObjObject(ObjModel parentModel) : this(parentModel, string.Empty)
        {
        }

        public ObjObject(ObjModel parentModel, string name)
        {
            if (parentModel == null)
            {
                throw new ArgumentNullException("parentModel", "Parent model cannot be null!");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name", "Name cannot be null!");
            }
            ListId = GL.GenLists(1);
            Name = name;
            Faces = new List<ObjFace>();
            ParentModel = parentModel;
        }

        public ObjModel ParentModel { get; private set; }
        public string Name { get; private set; }
        public List<ObjFace> Faces { get; private set; }
        public int ListId { get; private set; }

        /// <summary>
        /// Renders the object using the list. Usually called after Build();
        /// </summary>
        public void Render()
        {
            GL.CallList(ListId);
        }

        /// <summary>
        /// Builds the object into GPU using OPENGL lists.
        /// </summary>
        public void Build()
        {
            var lastFaceMaterial = string.Empty;
            GL.NewList(ListId, ListMode.Compile);

            int previousTexture;
            GL.GetInteger(GetPName.Texture2D, out previousTexture);

            foreach (var face in Faces)
            {
                if ((lastFaceMaterial == string.Empty) || (lastFaceMaterial != string.Empty && face.MaterialName != lastFaceMaterial))
                {
                    if (ParentModel.Materials.ContainsKey(face.MaterialName))
                    {
                        var material = ParentModel.Materials[face.MaterialName];
                        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, new[]
                        {
                            (float)material.AmbientColor.Red, 
                            (float)material.AmbientColor.Green, 
                            (float)material.AmbientColor.Blue, 
                            (float)material.Transparency
                        });
                        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, new[]
                        {
                            (float)material.DiffuseColor.Red, 
                            (float)material.DiffuseColor.Green, 
                            (float)material.DiffuseColor.Blue, 
                            (float)material.Transparency
                        });
                        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new[]
                        {
                            (float)material.SpecularColor.Red, 
                            (float)material.SpecularColor.Green, 
                            (float)material.SpecularColor.Blue, 
                            (float)material.Transparency
                        });
                        GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, (float)material.Shininess);
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, material.DiffuseTexture.Texture);
                        GL.ActiveTexture(TextureUnit.Texture1);
                        GL.BindTexture(TextureTarget.Texture2D, material.AmbientTexture.Texture);
                        GL.ActiveTexture(TextureUnit.Texture2);
                        GL.BindTexture(TextureTarget.Texture2D, material.SpecularTexture.Texture);
                    }
                }

                GL.ShadeModel(ShadingModel.Smooth);
                GL.Begin(PrimitiveType.Polygon);
                foreach (var faceItem in face.FaceItems)
                {
                    if (faceItem.NormalIndex != -1)
                    {
                        var normal = ParentModel.Normals[faceItem.NormalIndex];
                        GL.Normal3(normal.X, normal.Y, normal.Z);
                    }
                    if (faceItem.TextureIndex != -1)
                    {
                        var texture = ParentModel.Textures[faceItem.TextureIndex];
                        GL.TexCoord2(texture.X, texture.Y);
                    }
                    var vertex = ParentModel.Vertices[faceItem.VertexIndex];
                    GL.Vertex3(vertex.X, vertex.Y, vertex.Z);
                }
                GL.End();

                lastFaceMaterial = face.MaterialName;
            }
            GL.BindTexture(TextureTarget.Texture2D, previousTexture);
            GL.EndList();
        }

        public void Dispose()
        {
            GL.DeleteLists(ListId, 1);
        }
    }
}
