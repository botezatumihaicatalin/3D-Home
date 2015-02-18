using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Home3d.Model
{
    /// <summary>
    /// The class which holds all the information about the model :
    /// vertices , normals , textures , materials etc..
    /// </summary>
    public class ObjModel
    {
        public ObjModel()
        {
            Vertices = new List<ObjVertex>();
            ComputedNormals = new List<ObjNormal>();
            Normals = new List<ObjNormal>();
            Textures = new List<ObjTexture>();
            Objects = new Dictionary<string, ObjObject>();
            Materials = new Dictionary<string, ObjMaterial>();
            MaximumVertex = new ObjVertex();
            MinimumVertex = new ObjVertex();
        }

        public List<ObjVertex> Vertices { get; private set; }
        public List<ObjNormal> ComputedNormals { get; private set; } 
        public List<ObjNormal> Normals { get; private set; }
        public List<ObjTexture> Textures { get; private set; }
        public Dictionary<string, ObjObject> Objects { get; private set; }
        public Dictionary<string, ObjMaterial> Materials { get; private set; }

        public ObjVertex MinimumVertex { get; private set; }
        public ObjVertex MaximumVertex { get; private set; }

        /// <summary>
        /// Calculates the bounding box of the obj model.
        /// </summary>
        public void CalculateMaxAndMinVertex()
        {
            MinimumVertex.X = Double.MaxValue;
            MinimumVertex.Y = Double.MaxValue;
            MinimumVertex.Z = Double.MaxValue;

            MaximumVertex.X = Double.MinValue;
            MaximumVertex.Y = Double.MinValue;
            MaximumVertex.Z = Double.MinValue;

            foreach (var objVertex in Vertices)
            {
                if (MinimumVertex.X > objVertex.X)
                {
                    MinimumVertex.X = objVertex.X;
                }
                if (MinimumVertex.Y > objVertex.Y)
                {
                    MinimumVertex.Y = objVertex.Y;
                }
                if (MinimumVertex.Z > objVertex.Z)
                {
                    MinimumVertex.Z = objVertex.Z;
                }

                if (MaximumVertex.X < objVertex.X)
                {
                    MaximumVertex.X = objVertex.X;
                }
                if (MaximumVertex.Y < objVertex.Y)
                {
                    MaximumVertex.Y = objVertex.Y;
                }
                if (MaximumVertex.Z < objVertex.Z)
                {
                    MaximumVertex.Z = objVertex.Z;
                }
            }
        }

        /// <summary>
        /// Loads an mtl file.
        /// </summary>
        /// <param name="objMaterialPath">The path to the mtl file.</param>
        public void LoadMaterial(string objMaterialPath)
        {
            if (!File.Exists(objMaterialPath))
            {
                throw new ArgumentException("File does not exist!", objMaterialPath);
            }

            var objMaterialFolder = Path.GetDirectoryName(objMaterialPath) ?? string.Empty;
            var objMaterialStreamReader = new StreamReader(objMaterialPath);
            string lastMaterialNameRead = null;
            string originalLine = null;
            const char lineSeparator = ' ';

            while ((originalLine = objMaterialStreamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(originalLine))
                {
                    continue;
                }

                var trimmedLine = originalLine.Trim();
                var splittedLine = trimmedLine.Split(lineSeparator);
                var splittedLineSize = splittedLine.Length;

                if (trimmedLine[0] == '#')
                {
                    continue;
                }

                if (splittedLineSize == 1)
                {
                    throw new ParseException(originalLine, "Arguments from the token are missing.");
                }

                if (splittedLine[0] == "newmtl" && splittedLineSize >= 2)
                {
                    var materialName = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                    lastMaterialNameRead = materialName;
                    Materials[lastMaterialNameRead] = new ObjMaterial(lastMaterialNameRead);
                    continue;
                }

                if (splittedLine[0] == "Ka" && splittedLineSize >= 4)
                {
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    double r, g, b;
                    var resultR = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out r);
                    var resultG = double.TryParse(splittedLine[2], NumberStyles.Any, CultureInfo.InvariantCulture, out g);
                    var resultB = double.TryParse(splittedLine[3], NumberStyles.Any, CultureInfo.InvariantCulture, out b);
                    if (!(resultR && resultG && resultB))
                    {
                        throw new ParseException(originalLine, "Ambient rgb values must be floating point numbers.");
                    }
                    material.AmbientColor.Red = r;
                    material.AmbientColor.Green = g;
                    material.AmbientColor.Blue = b;
                    continue;
                }

                if (splittedLine[0] == "Kd" && splittedLineSize >= 4)
                {
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    double r, g, b;
                    var resultR = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out r);
                    var resultG = double.TryParse(splittedLine[2], NumberStyles.Any, CultureInfo.InvariantCulture, out g);
                    var resultB = double.TryParse(splittedLine[3], NumberStyles.Any, CultureInfo.InvariantCulture, out b);
                    if (!(resultR && resultG && resultB))
                    {
                        throw new ParseException(originalLine, "Ambient rgb values must be floating point numbers.");
                    }
                    material.DiffuseColor.Red = r;
                    material.DiffuseColor.Green = g;
                    material.DiffuseColor.Blue = b;
                    continue;
                }

                if (splittedLine[0] == "Ks" && splittedLineSize >= 4)
                {
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    double r, g, b;
                    var resultR = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out r);
                    var resultG = double.TryParse(splittedLine[2], NumberStyles.Any, CultureInfo.InvariantCulture, out g);
                    var resultB = double.TryParse(splittedLine[3], NumberStyles.Any, CultureInfo.InvariantCulture, out b);
                    if (!(resultR && resultG && resultB))
                    {
                        throw new ParseException(originalLine, "Ambient rgb values must be floating point numbers.");
                    }
                    material.SpecularColor.Red = r;
                    material.SpecularColor.Green = g;
                    material.SpecularColor.Blue = b;
                    continue;
                }

                if (splittedLine[0] == "Ns" && splittedLineSize >= 2)
                {
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    double ns;
                    var result = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out ns);
                    if (!result)
                    {
                        throw new ParseException(originalLine, "Ns value must be a floating point number.");
                    }
                    material.Shininess = ns;
                    continue;
                }

                if ((splittedLine[0] == "d" || splittedLine[0] == "Tr")
                        && splittedLineSize >= 2)
                {
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    double transparency;
                    var result = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture,
                        out transparency);
                    if (!result)
                    {
                        throw new ParseException(originalLine, "Transparency value must be a floating point number.");
                    }
                    material.Transparency = transparency;
                    continue;
                }

                if (splittedLine[0] == "illum" && splittedLineSize >= 2)
                {
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    int illum;
                    var result = int.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out illum);
                    if (!result)
                    {
                        throw new ParseException(originalLine, "Illum value must be a integer number.");
                    }
                    material.Illumination = illum;
                    continue;
                }

                if (splittedLine[0] == "map_Ka" && splittedLineSize >= 2)
                {
                    // TODO : Need to add all options here, for complete parsing.
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    if (splittedLine.Length >= 6 && splittedLine[1] == "-o")
                    {
                        material.AmbientTexture.ScaleU = double.Parse(splittedLine[2], CultureInfo.InvariantCulture);
                        material.AmbientTexture.ScaleV = double.Parse(splittedLine[3], CultureInfo.InvariantCulture);
                        var texturePath = string.Join(new string(lineSeparator, 1), splittedLine.Skip(5));
                        material.AmbientTexture.LoadImage(Path.Combine(objMaterialFolder, texturePath));
                    }
                    else
                    {
                        var texturePath = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                        material.AmbientTexture.LoadImage(Path.Combine(objMaterialFolder, texturePath));
                    }
                    continue;
                }

                if (splittedLine[0] == "map_Kd" && splittedLineSize >= 2)
                {
                    // TODO : Need to add all options here, for complete parsing.
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    if (splittedLine.Length >= 6 && splittedLine[1] == "-o")
                    {
                        material.DiffuseTexture.ScaleU = double.Parse(splittedLine[2], CultureInfo.InvariantCulture);
                        material.DiffuseTexture.ScaleV = double.Parse(splittedLine[3], CultureInfo.InvariantCulture);
                        var texturePath = string.Join(new string(lineSeparator, 1), splittedLine.Skip(5));
                        material.DiffuseTexture.LoadImage(Path.Combine(objMaterialFolder, texturePath));
                    }
                    else
                    {
                        var texturePath = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                        material.DiffuseTexture.LoadImage(Path.Combine(objMaterialFolder, texturePath));
                    }
                    continue;
                }

                if (splittedLine[0] == "map_Ks" && splittedLineSize >= 2)
                {
                    // TODO : Need to add all options here, for complete parsing.
                    if (lastMaterialNameRead == null)
                    {
                        throw new ParseException(originalLine, "No material was defined before.");
                    }
                    if (!Materials.ContainsKey(lastMaterialNameRead))
                    {
                        throw new ParseException(originalLine, string.Format("Material {0} is undefined.", lastMaterialNameRead));
                    }
                    var material = Materials[lastMaterialNameRead];
                    if (splittedLine.Length >= 6 && splittedLine[1] == "-o")
                    {
                        material.SpecularTexture.ScaleU = double.Parse(splittedLine[2], CultureInfo.InvariantCulture);
                        material.SpecularTexture.ScaleV = double.Parse(splittedLine[3], CultureInfo.InvariantCulture);
                        var texturePath = string.Join(new string(lineSeparator, 1), splittedLine.Skip(5));
                        material.SpecularTexture.LoadImage(Path.Combine(objMaterialFolder, texturePath));
                    }
                    else
                    {
                        var texturePath = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                        material.SpecularTexture.LoadImage(Path.Combine(objMaterialFolder, texturePath));
                    }
                    continue;
                }

                if (splittedLine[0] == "Ni")
                {
                    continue;
                }

                throw new ParseException(originalLine, "Unable to parse because originalLine doesnt fit into the mtl format.");
            }
        }

        /// <summary>
        /// Loads an obj file.
        /// </summary>
        /// <param name="objModelPath">The path to the obj file.</param>
        /// <throws>
        /// ParseException
        /// ArgumentException
        /// </throws>
        public void Load(string objModelPath)
        {
            if (!File.Exists(objModelPath))
            {
                throw new ArgumentException("File does not exist!", objModelPath);
            }

            var objModelFolder = Path.GetDirectoryName(objModelPath) ?? string.Empty;
            var objModelStreamReader = new StreamReader(objModelPath);

            string originalLine;
            string lastObjectMaterialRead = null;
            string lastObjectNameRead = null;
            const char lineSeparator = ' ';

            while ((originalLine = objModelStreamReader.ReadLine()) != null)
            {
                
                if (string.IsNullOrWhiteSpace(originalLine))
                {
                    continue;
                }

                var trimmedLine = originalLine.Trim();
                var splittedLine = trimmedLine.Split(lineSeparator);
                var splittedLineSize = splittedLine.Length;

                if (trimmedLine[0] == '#')
                {
                    continue;
                }

                if (splittedLineSize == 1)
                {
                    throw new ParseException(originalLine, "Arguments from the token are missing.");
                }

                if (splittedLine[0] == "mtllib" && splittedLineSize >= 2)
                {
                    var materialFileName = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                    LoadMaterial(Path.Combine(objModelFolder, materialFileName));
                    continue;
                }

                if (splittedLine[0] == "v" && splittedLineSize >= 4)
                {
                    double x, y, z;
                    var resultX = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    var resultY = double.TryParse(splittedLine[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    var resultZ = double.TryParse(splittedLine[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                    if (!(resultX && resultY && resultZ))
                    {
                        throw new ParseException(originalLine, "Vertexes coordinates must be floating point numbers.");
                    }
                    Vertices.Add(new ObjVertex {X = x , Y = y, Z = z});
                    continue;
                }

                if (splittedLine[0] == "vt" && splittedLineSize >= 3)
                {
                    double x, y;
                    var resultX = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    var resultY = double.TryParse(splittedLine[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    if (!(resultX && resultY))
                    {
                        throw new ParseException(originalLine, "Texture coordinates must be floating point numbers.");
                    }
                    Textures.Add(new ObjTexture {X = x, Y = y});
                    continue;
                }

                if (splittedLine[0] == "vn" && splittedLineSize >= 4)
                {
                    double x, y, z;
                    var resultX = double.TryParse(splittedLine[1], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    var resultY = double.TryParse(splittedLine[2], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                    var resultZ = double.TryParse(splittedLine[3], NumberStyles.Any, CultureInfo.InvariantCulture, out z);
                    if (!(resultX && resultY && resultZ))
                    {
                        throw new ParseException(originalLine, "Vertexes coordinates must be floating point numbers.");
                    }
                    Normals.Add(new ObjNormal {X = x, Y = y, Z = z});
                    continue;
                }

                if (splittedLine[0] == "f")
                {
                    if (Objects.Count == 0)
                    {
                        Objects["Untitled"] = new ObjObject(this, "Untitled");
                        lastObjectNameRead = "Untitled";
                    }

                    var newFace = new ObjFace(lastObjectMaterialRead);

                    for (var index = 1; index < splittedLineSize; index++)
                    {
                        var faceItem = new ObjFaceItem();
                        var tokens = splittedLine[index].Split('/');

                        if (tokens.Length >= 1)
                        {
                            if (string.IsNullOrWhiteSpace(tokens[0]))
                            {
                                throw new ParseException(originalLine , "Faces need at least the vertex index to be present.");
                            }
                            int vertexIndex;
                            var result = int.TryParse(tokens[0], NumberStyles.Any, CultureInfo.InvariantCulture, out vertexIndex);
                            if (!result)
                            {
                                throw new ParseException(originalLine, "Faces require vertex index to be an integer value.");
                            }
                            faceItem.VertexIndex = vertexIndex - 1;
                        }
                        if (tokens.Length >= 2 && !string.IsNullOrWhiteSpace(tokens[1]))
                        {
                            int textureIndex;
                            var result = int.TryParse(tokens[1], NumberStyles.Any, CultureInfo.InvariantCulture, out textureIndex);
                            if (!result)
                            {
                                throw new ParseException(originalLine, "Faces require texture index to be an integer value.");
                            }
                            faceItem.TextureIndex = textureIndex - 1;
                        }
                        if (tokens.Length >= 3 && !string.IsNullOrWhiteSpace(tokens[2]))
                        {
                            int normalIndex;
                            var result = int.TryParse(tokens[2], NumberStyles.Any, CultureInfo.InvariantCulture, out normalIndex);
                            if (!result)
                            {
                                throw new ParseException(originalLine, "Faces require normal index to be an integer value.");
                            }
                            faceItem.NormalIndex = normalIndex - 1;
                        }
                        newFace.FaceItems.Add(faceItem);
                    }

                    Objects[lastObjectNameRead].Faces.Add(newFace);
                    continue;
                }

                if (splittedLine[0] == "o" && splittedLineSize >= 2)
                {
                    var objectName = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                    Objects[objectName] = new ObjObject(this, objectName);
                    lastObjectNameRead = objectName;
                    continue;
                }

                if (splittedLine[0] == "usemtl" && splittedLineSize >= 2)
                {
                    var materialName = string.Join(new string(lineSeparator, 1), splittedLine.Skip(1));
                    if (!Materials.ContainsKey(materialName))
                    {
                        throw new ParseException(originalLine , string.Format("Unable to find the material {0}", materialName));
                    }
                    lastObjectMaterialRead = materialName;
                    continue;
                }

                if (splittedLine[0] == "g")
                {
                    // TODO : maybe handle groups?
                    continue;
                }

                if (splittedLine[0] == "s")
                {
                    // TODO : maybe handle smoothness (not necesary)
                    continue;
                }
                throw new ParseException(originalLine, "Unable to parse because originalLine doesnt fit into the obj format.");
            }

            // Calulcate min vetexes and max vertexes
            CalculateMaxAndMinVertex();

            foreach (var objObject in Objects.Values)
            {
                objObject.Build();
            }
        }
    }
}
