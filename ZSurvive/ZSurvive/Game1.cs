#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ZSurvive
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;		
		Vector2 Position;
		SpriteFont fuente1;
		Texture2D Fondo,Player,Z1,Mira,Img1,Img2,Img3,Img4;
		Rectangle Zombie1,PlayerR;
		Color color = Color.White;
		bool disp=true, pause=true,over=false,musicp=false,musicn1=false,musicn2=false,musicf=false;
		int ancho, alto, balas=12,resZ=2,ZM=0,Nivel=1,ZP=0,D=0;
		float TR=50,angle;
		MouseState oldMouse;
		KeyboardState oldKey;
		SoundEffect PistoS,Recarga;
		Song N1,N2,P,F;
		List<Vector2> posicionesEnemigo = new List<Vector2> ();
		double probabilidadEnemigo = 0.02;
		Random aleatorio = new Random ();

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../Content";            
			graphics.IsFullScreen = false;		
		}

		protected override void Initialize()
		{
			Position = new Vector2(500, 350);
			alto = 700;
			ancho = 1000;
			graphics.PreferredBackBufferHeight = alto;
			graphics.PreferredBackBufferWidth = ancho;
			oldMouse = Mouse.GetState();
			oldKey = Keyboard.GetState ();
			base.Initialize();

		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			PistoS = Content.Load<SoundEffect> ("Audio/PistoS");
			Recarga = Content.Load<SoundEffect> ("Audio/Recarga");
			fuente1 = Content.Load<SpriteFont> ("Fuentes/fuente1");
			Z1 = Content.Load<Texture2D> ("Zombies/Z1");
			Img1 = Content.Load<Texture2D> ("Imagenes/1");
			Img2 = Content.Load<Texture2D> ("Imagenes/2");
			Img3 = Content.Load<Texture2D> ("Imagenes/3");
			Img4 = Content.Load<Texture2D> ("Imagenes/Over");
			Player = Content.Load<Texture2D> ("Jugador/Pisto");
			Mira = Content.Load<Texture2D> ("Imagenes/Mira");
		}

		protected override void Update(GameTime gameTime)
		{
			//Limitaciones de la pantalla
			Position.X = MathHelper.Clamp (Position.X, 15, ancho-15);
			Position.Y = MathHelper.Clamp (Position.Y, 15, alto-15); 

			KeyboardState keyState = Keyboard.GetState ();
			if (keyState.IsKeyDown (Keys.Escape))
				Exit ();

			//Pausa
			if (keyState.IsKeyDown (Keys.P)) 
					pause = true;
			if (keyState.IsKeyDown (Keys.Space)) {
				pause = false;

			}
			if (pause == true) {
				if (musicp == false) {
					P = Content.Load<Song> ("Musica/Lzn02-The Dolls Death.wav");
					MediaPlayer.Play (P);
					musicp = true;
				}
				return;
			}
			if ((pause == false) && (Nivel == 1)) {
				if (musicn1 == false) {
					N1 = Content.Load<Song> ("Musica/Nosphares-Strengths Of Evil.wav");
					MediaPlayer.Play (N1);
					musicn1 = true;
				}
			}
			if ((pause == false) && (Nivel == 2)) {
				if (musicn2 == false) {
					N2 = Content.Load<Song> ("Musica/Shamatronic-Jack the Pirate.wav");
					MediaPlayer.Play (N2);
					musicn2 = true;
				}
			}
			if ((pause == true) && (over == true)) {
				if (musicf == false) {
					F = Content.Load<Song> ("Musica/Akashic-Stories of The Old Mansion.wav");
					MediaPlayer.Play (F);
					musicf = true;
				}
			}
			//Teclado
			MouseState newMouse = Mouse.GetState ();
			if (keyState.IsKeyDown (Keys.A))
				Position.X -= 2.5f;
			if (keyState.IsKeyDown (Keys.D))
				Position.X += 2.5f;
			if (keyState.IsKeyDown (Keys.W))
				Position.Y -= 2.5f;
			if (keyState.IsKeyDown (Keys.S))
				Position.Y += 2.5f;	
			if (keyState.IsKeyDown (Keys.S) && keyState.IsKeyDown (Keys.A))
			{
				Position.Y -= 0.80f;
				Position.X += 0.80f;
			}
			if (keyState.IsKeyDown (Keys.S) && keyState.IsKeyDown (Keys.D))
			{
				Position.Y -= 0.80f;
				Position.X -= 0.80f;
			}
			if (keyState.IsKeyDown (Keys.W) && keyState.IsKeyDown (Keys.A))
			{
				Position.Y += 0.80f;
				Position.X += 0.80f;
			}
			if (keyState.IsKeyDown (Keys.W) && keyState.IsKeyDown (Keys.D))
			{
				Position.Y += 0.80f;
				Position.X -= 0.80f;
			}

			//Recarga
			if (keyState.IsKeyDown (Keys.R)) {
				if (!oldKey.IsKeyDown (Keys.R)) {
					Recarga.Play ();
					balas = 12;
				}
			}
			if (balas <= 0)
				disp = false;
			if (balas > 0)
				disp = true;

			TR = TR - (float)(gameTime.ElapsedGameTime.TotalSeconds);

			PlayerR =
				new Rectangle ((int)Position.X, (int)Position.Y,
				               Player.Width, Player.Height);

			if (Nivel == 1) {
				Fondo = Content.Load<Texture2D> ("Imagenes/Pasto");
				if (aleatorio.NextDouble () < probabilidadEnemigo) { 
					float y = (float)aleatorio.NextDouble () * 
						700; 
					posicionesEnemigo.Add (new Vector2 (0, y)); 
				}
				for (int i = 0; i < posicionesEnemigo.Count; i++) { 
					posicionesEnemigo [i] = new Vector2 (posicionesEnemigo [i].X + 0.8f, 
					                                     posicionesEnemigo [i].Y);

					Zombie1 = new Rectangle ((int)posicionesEnemigo [i].X + 15,
					                         (int)posicionesEnemigo [i].Y + 15,
					                         Z1.Width, Z1.Height);

					if ((PlayerR.Intersects (Zombie1)) || (ZP==5))
						over = true;

					if (posicionesEnemigo [i].X > Window.ClientBounds.Width) { 
						posicionesEnemigo.RemoveAt (i);
						i--;
						ZP++;
					}
					if (disp == true) {
						if (newMouse.LeftButton == ButtonState.Pressed) {
							if (oldMouse.LeftButton != ButtonState.Pressed) {
								if (new Rectangle (Mouse.GetState ().X + 11, Mouse.GetState ().Y + 11, 1, 1).Intersects (Zombie1)) {
									resZ--;
									D++;
									if (resZ == 0) {
										posicionesEnemigo.RemoveAt (i);
										i--;
										ZM++;
										resZ = 2;
									}
								} else if (oldMouse.LeftButton == ButtonState.Pressed) {
									Player = Content.Load<Texture2D> ("Jugador/Pisto");
								}
							}
						}
					}
				}

				if (TR <= 0) {
					posicionesEnemigo.Clear ();
					Position = new Vector2 (500, 375);
					Nivel++;
					Z1 = Content.Load<Texture2D> ("Zombies/Z2");
					Position = new Vector2 (500, 375);
					balas = 12;
					pause = true;
					TR=70;
				}
			}
			if (Nivel == 2) {
				Fondo = Content.Load<Texture2D> ("Imagenes/Tierra");
				if (aleatorio.NextDouble () < probabilidadEnemigo) { 
					float x = (float)aleatorio.NextDouble () * 
						Window.ClientBounds.Height; 
					posicionesEnemigo.Add (new Vector2 (x, 0)); 
				}
				for (int i = 0; i < posicionesEnemigo.Count; i++) { 
					posicionesEnemigo [i] = new Vector2 (posicionesEnemigo [i].X, 
					                                     posicionesEnemigo [i].Y+1);

					Zombie1 = new Rectangle ((int)posicionesEnemigo [i].X + 10,
					                         (int)posicionesEnemigo [i].Y + 15,
					                         Z1.Width, Z1.Height);

					if ((PlayerR.Intersects (Zombie1)) || (ZP==5))
						over = true;

					if (posicionesEnemigo [i].Y > Window.ClientBounds.Height) { 
						posicionesEnemigo.RemoveAt (i);
						i--;
						ZP++;
					}
					if (disp == true) {
						if (newMouse.LeftButton == ButtonState.Pressed) {
							if (oldMouse.LeftButton != ButtonState.Pressed) {
								if (new Rectangle (Mouse.GetState ().X + 11, Mouse.GetState ().Y + 11, 1, 1).Intersects (Zombie1)) {
									resZ--;
									D++;
									if (resZ == 0) {
										posicionesEnemigo.RemoveAt (i);
										i--;
										ZM++;
										resZ = 2;
									}
								} else if (oldMouse.LeftButton == ButtonState.Pressed) {
									Player = Content.Load<Texture2D> ("Jugador/Pisto");
								}
							}
						}
					}
				}
			}
			if (disp == true) {
				if (newMouse.LeftButton == ButtonState.Pressed) {
					if (oldMouse.LeftButton != ButtonState.Pressed) {
						balas--;
						PistoS.Play ();
						Player = Content.Load<Texture2D> ("Jugador/PistoA");
					}
				} else if (oldMouse.LeftButton == ButtonState.Pressed) {
					Player = Content.Load<Texture2D> ("Jugador/Pisto");
				}
			}

			oldKey = keyState;
			oldMouse = newMouse;
			Vector2 mouseLoc = new Vector2 (newMouse.X, newMouse.Y);
			Vector2 direction = mouseLoc - Position;
			angle = (float)(Math.Atan2 (direction.Y, direction.X));
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Black);
			spriteBatch.Begin ();

			if ((pause == true) && (TR == 70)) {
				spriteBatch.Draw (Img2, Vector2.Zero,Color.White);
				spriteBatch.DrawString (fuente1, "La ayuda ha llegado, pero mientras se encargan de los zombies restantes", new Vector2 (40, 40), Color.Red);
				spriteBatch.DrawString (fuente1, "en la zona oeste, te han pedido que retengas la horda que se acerca", new Vector2 (40, 55), Color.Red);
				spriteBatch.DrawString (fuente1, "por el norte, resiste tanto como puedas...", new Vector2 (40, 70), Color.Red);
				spriteBatch.DrawString (fuente1, "Presiona espacio para continuar", new Vector2 (40, 85), Color.Red);
			}
			if ((pause == true) && (TR!=50))
				spriteBatch.DrawString (fuente1, "Presiona espacio para continuar", new Vector2 (40, 85), Color.Red);

			if ((pause == true) && (TR == 50)) {
				spriteBatch.Draw (Img1, Vector2.Zero,Color.White);
				spriteBatch.DrawString (fuente1, "Eres un militar retirado con mucha experiencia. Sin explicacion alguna (aún)", new Vector2 (40, 40), Color.Red);
				spriteBatch.DrawString (fuente1, "los muertos estan despertando de su descanso y se acercan a tu pueblo, los", new Vector2 (40, 55), Color.Red);
				spriteBatch.DrawString (fuente1, "refuerzos vienen en camino pero por el momento el pueblo solo cuenta contigo y", new Vector2 (40, 70), Color.Red);
				spriteBatch.DrawString (fuente1, "con tu vieja pistola. Te diriges  al lado oeste del pueblo donde una horda se acerca...", new Vector2 (40, 85), Color.Red);
				spriteBatch.DrawString (fuente1, "Movimiento con: W,A,S,D", new Vector2 (40, 375), Color.Red);
				spriteBatch.DrawString (fuente1, "Recargar pistola con: R", new Vector2 (40, 390), Color.Red);
				spriteBatch.DrawString (fuente1, "Pausar el juego: P", new Vector2 (40, 405), Color.Red);
				spriteBatch.DrawString (fuente1, "Apuntar con el mouse y disparar con clic izquierdo", new Vector2 (40, 420), Color.Red);
				spriteBatch.DrawString (fuente1, "¡Ten en cuenta que si un zombie te atrapa una vez, o pasan 5 zombies al pueblo, pierdes!", new Vector2 (40, 440), Color.Red);
				spriteBatch.DrawString (fuente1, "Presiona 'Espacio' para comenzar", new Vector2 (250, 470), Color.Red);
			}
		
			if (pause == false) {
				spriteBatch.Draw (Fondo, Vector2.Zero, Color.White);
				spriteBatch.Draw (Player, PlayerR, null, color, angle, new Vector2 (Player.Width / 2, Player.Height / 2), SpriteEffects.None, 0);
				foreach (Vector2 posicionEnemigo in posicionesEnemigo) { 
					spriteBatch.Draw (Z1, posicionEnemigo, Color.White);
				}
				spriteBatch.DrawString (fuente1, "Zombies que han atravesado al pueblo: " + ZP, new Vector2 (5, 5), Color.Black);
				spriteBatch.DrawString (fuente1, "Tiempo restante: " + TR, new Vector2 (475, 5), Color.Black);
				spriteBatch.DrawString (fuente1, "Nivel: " + Nivel, new Vector2 (900, 5), Color.Black);
				spriteBatch.DrawString (fuente1, "Balas: " + balas, new Vector2 (900, 650), Color.Black);
				spriteBatch.DrawString (fuente1, "Zombies asesinados: " + ZM, new Vector2 (5, 650), Color.Black);
				spriteBatch.Draw (Mira, new Vector2 (Mouse.GetState ().X - 25, Mouse.GetState ().Y - 25), Color.White);
			}
			if ((Nivel == 2) && (TR <= 0)) {
				pause = true;
				posicionesEnemigo.Clear ();
				spriteBatch.Draw (Img3, Vector2.Zero,Color.White);
				spriteBatch.DrawString (fuente1, "Felicitaciones, has logrado rechazar las hordas... por ahora.", new Vector2 (5, 5), Color.Red);
				spriteBatch.DrawString (fuente1, "Zombies asesinados: " + ZM, new Vector2 (5, 200), Color.Red);
				spriteBatch.DrawString (fuente1, "Disparos: " + D, new Vector2 (5, 185), Color.Red);
				Console.ReadKey ();
			}
			if (over == true) {
				pause = true;
				posicionesEnemigo.Clear ();
				spriteBatch.Draw (Img4, Vector2.Zero,Color.White);
				spriteBatch.DrawString (fuente1, "Fin del juego...", new Vector2 (5, 5), Color.Red);
				spriteBatch.DrawString (fuente1, "Zombies asesinados: " + ZM, new Vector2 (5, 150), Color.Red);
				spriteBatch.DrawString (fuente1, "Disparos: " + D, new Vector2 (5, 185), Color.Red);
				Console.ReadKey ();
			}
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}