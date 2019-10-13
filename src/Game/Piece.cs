using Core.Extensions;
using UnityEngine;

namespace Game
{
    public class Piece : Instance
    {
        public Vector3Int StickerDirections { get; }
        
        public Piece(Vector3Int stickerDirections, GameObject parent = null)
            : base("{0} {1}".Format(nameof(Piece), stickerDirections), parent)
        {
            StickerDirections = stickerDirections;

            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f);
            SetAsChild(cube);

            if (stickerDirections.x == 1)
            {
                var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sticker.transform.localScale = new Vector3(0.1f, 0.8f, 0.8f);
                sticker.transform.localPosition = Vector3.right * 0.5f;
                sticker.GetComponent<Renderer>().material.color = Color.red;
                SetAsChild(sticker);
                
            } else if (stickerDirections.x == -1)
            {
                var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sticker.transform.localScale = new Vector3(0.1f, 0.8f, 0.8f);
                sticker.transform.localPosition = Vector3.left * 0.5f;
                sticker.GetComponent<Renderer>().material.color = Color.green;
                SetAsChild(sticker);
            }
            
            if (stickerDirections.y == 1)
            {
                var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sticker.transform.localScale = new Vector3(0.8f, 0.1f, 0.8f);
                sticker.transform.localPosition = Vector3.up * 0.5f;
                sticker.GetComponent<Renderer>().material.color = Color.blue;
                SetAsChild(sticker);
                
            } else if (stickerDirections.y == -1)
            {
                var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sticker.transform.localScale = new Vector3(0.8f, 0.1f, 0.8f);
                sticker.transform.localPosition = Vector3.down * 0.5f;
                sticker.GetComponent<Renderer>().material.color = Color.yellow;
                SetAsChild(sticker);
            }
            
            if (stickerDirections.z == 1)
            {
                var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sticker.transform.localScale = new Vector3(0.8f, 0.8f, 0.1f);
                sticker.transform.localPosition = Vector3.forward * 0.5f;
                sticker.GetComponent<Renderer>().material.color = Color.white;
                SetAsChild(sticker);
                
            } else if (stickerDirections.z == -1)
            {
                var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sticker.transform.localScale = new Vector3(0.8f, 0.8f, 0.1f);
                sticker.transform.localPosition = Vector3.back * 0.5f;
                sticker.GetComponent<Renderer>().material.color = Color.magenta;
                SetAsChild(sticker);
            }
        }
    }
}