using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareTime {
    class UICanvas {
        private Camera cam;
        private List<Button> buttons;
        private SpriteBatch texturePuncher;
        private Button selectedOption;
        public UICanvas(Camera cam) {
            buttons = new List<Button>();
            texturePuncher = new SpriteBatch(SquareGame.GraphicsD);
            this.cam = cam;
        }
        public void Update() {
            if (InputManager.getMouseClick()) {//check for any mouse buttons
                foreach (Button b in buttons) {
                    Vector2 mousePos = InputManager.getMousePos();
                    if (mousePos.X < b.screenPos.X) continue;
                    if (mousePos.X > b.screenPos.X + b.baseTexture.Width) continue;
                    if (mousePos.Y < b.screenPos.Y) continue;
                    if (mousePos.Y > b.screenPos.Y + b.baseTexture.Height) continue;
                    if (b is SelectableButton) {
                        (b as SelectableButton).onClickAction(b);
                    }
                    else {
                        if (b == selectedOption) {
                            selectedOption = null; b.selected = false;
                        }
                        else {
                            if (selectedOption != null) {
                                selectedOption.selected = false;
                            }
                            selectedOption = b;
                            b.selected = true;
                        }
                        return;
                    }
                }
                if (selectedOption is ToggleableButton) {
                    (selectedOption as ToggleableButton).onActiveClick(cam.screenToWorld(InputManager.getMousePos()));
                }
                if (selectedOption is DragBoxButton) {
                    (selectedOption as DragBoxButton).StartDrag(cam.screenToWorld(InputManager.getMousePos()));
                }
            }
            if (InputManager.getMouseUp() && selectedOption is DragBoxButton) {
                if ((selectedOption as DragBoxButton).Dragging) {
                    (selectedOption as DragBoxButton).onReleaseClick((selectedOption as DragBoxButton).startPoint, cam.screenToWorld(InputManager.getMousePos()));
                }
            }
            
        }
        public void addButton(Button b) {
            buttons.Add(b);
        }
        public void punchButtonSprite(Button b,string objID) {
            b.ButtonSprite = new RenderTarget2D(SquareGame.GraphicsD, b.baseTexture.Width, b.baseTexture.Height);
            SquareGame.GraphicsD.SetRenderTarget(b.ButtonSprite);
            texturePuncher.Begin();
            texturePuncher.Draw(b.baseTexture, Vector2.Zero, Color.White);
            texturePuncher.Draw(TextureBank.textureBank[objID], new Vector2(b.baseTexture.Width / 2, b.baseTexture.Height / 2), null,Color.White,0.0f,new Vector2(TextureBank.textureBank[objID].Width/2, TextureBank.textureBank[objID].Height / 2),32.0f/((float) TextureBank.textureBank[objID].Width),0,0);
            texturePuncher.End();
            SquareGame.GraphicsD.SetRenderTarget(null);
        }
        public void Draw(SpriteBatch batch) {
            if (selectedOption != null) {
                if (selectedOption.selected) {
                    if (selectedOption is ToggleableButton)
                        if((selectedOption as ToggleableButton).onActive != null)
                        (selectedOption as ToggleableButton).onActive(cam.screenToWorld(InputManager.getMousePos()));

                    if (selectedOption is DragBoxButton) {
                        DragBoxButton db = (selectedOption as DragBoxButton);
                        if ((selectedOption as DragBoxButton).onDragHover != null) {
                            if (db.Dragging) db.onDragHover(db.startPoint, cam.screenToWorld(InputManager.getMousePos()));
                            else db.onDragHover(cam.screenToWorld(InputManager.getMousePos()), cam.screenToWorld(InputManager.getMousePos()));
                        }
                    }
                }
            }
            foreach (Button b in buttons) {
                b.Draw(batch);
            }
        }
    }
    class HorizontalLayout {
        int hRowCount = 0;
        float spacing = 0.0f;
        Vector2 hRowAnchor;
        UICanvas canvas;
        public HorizontalLayout(UICanvas canvas,Vector2 anchor,float spacing = 0.0f) {
            this.canvas = canvas;
            this.hRowAnchor = anchor;
            this.spacing = spacing;
        }
        public void addPreppedButtonToLayout(Button b, string objId = null) {
            addButtonToLayout(b);
            canvas.punchButtonSprite(b, objId);
        }
        public void addButtonToLayout(Button b) {
            b.screenPos = new Vector2(hRowAnchor.X + (2 * spacing + b.baseTexture.Width) * hRowCount, hRowAnchor.Y);
            canvas.addButton(b);
            hRowCount++;
        }
    }



}
