using System;
using System.Collections;
using rubik.Core.Logging;
using UnityEngine;
using ILogger = rubik.Core.Logging.ILogger;

namespace rubik.Core
{
    public class RubiksCube
    {
        private readonly ILogger logger;
        private readonly GameObject proto;

        public RubiksCube()
        {
            logger = PrefixedLogger.ForType<RubiksCube>(new UnityConsoleLogger());
            proto = GameObject.CreatePrimitive(PrimitiveType.Cube);
            proto.SetActive(false);
        }
        
        public IEnumerator Generate(int cubesPerRow, Func<GameObject, GameObject> instantiator)
        {
            logger.Info("About to generate {0}x{0} rubik's cube...", cubesPerRow);

            yield return new WaitForSeconds(5);

            for (var i = 0; i < cubesPerRow; ++i)
            {
                for (var j = 0; j < cubesPerRow; ++j)
                {
                    for (var k = 0; k < cubesPerRow; ++k)
                    {
                        logger.Info("Gen {0},{1}", i, j);
                        var o = instantiator(proto);
                        o.GetComponent<Transform>().position = new Vector3(i, j, k) * 1.1f;
                        o.SetActive(true);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }
        }
    }
}