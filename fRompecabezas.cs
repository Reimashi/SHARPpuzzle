using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rompecabezas
{
    public partial class fRompecabezas : Form
    {
        /* ========================= Definición de variables para la lógica del programa ===================================== */

        private int[] estadoBotones = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        private Bitmap[] subimagenes = { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        private int movimientos = 0;

        public fRompecabezas()
        {
            InitializeComponent();
            this.toolStripComboBox1.SelectedIndex = 0;
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aitor González Fernández\nhttp://github.com/Reimashi/SHARPpuzzle\nRompecabezas 0.1a (Rev.02)", "Acerca de...");
        }

        private void Tablero_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                if (sender.Equals(botones[i]))
                {
                    if (moverFicha(i))
                    {
                        if (comprobarColocado())
                        {
                            this.toolStripStatusLabel1.Text = "Enhorabuena, has ganado";
                            deshabilitarBotones();
                        }
                        else
                        {
                            this.toolStripStatusLabel1.Text = " Movimientos: " + movimientos;
                        }
                    }
                    else
                    {
                        this.toolStripStatusLabel1.Text = " Movimiento incorrecto";
                    }
                }
            }
        }

        private void SelectorImagen_Click(object sender, EventArgs e)
        {
            switch (this.toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    getImagenes(".\\images\\paisaje.jpg");
                    break;

                case 1:
                    getImagenes(".\\images\\cuna.png");
                    break;

                case 2:
                    getImagenes(".\\images\\montanas.png");
                    break;

                case 3:
                    PersonalizarImagen_Click(sender, e);
                    break;

                default:
                    getImagenes(".\\images\\paisaje.png");
                    break;
            }

            nuevoJuego();
        }

        private void NuevoJuego_Click(object sender, EventArgs e)
        {
            nuevoJuego();
        }

        private void PersonalizarImagen_Click(object sender, EventArgs e)
        {
            cambiarImagen();
        }

        /* ====================================== Logica del programa ========================================================== */

        //
        // Metodo para cambiar de imagen con dialogo de seleccion
        //
        private void cambiarImagen()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Imagen (PNG)|*.png|Imagen (JPG)|*.jpg|Imagen (JPEG)|*.jpeg|Imagen (GIF)|*.gif|Imagen (BMP)|*.bmp|Todos|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    getImagenes(openFileDialog1.FileName);
                    nuevoJuego();
                    // Borrar la seleccion en el bar
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: No se puede leer el archivo:\n" + ex.Message, "Error!!!");
                }
            }
        }


        //
        // Metodo que cambia la imagen y inicia un nuevo juego
        //
        private void cambiarImagen(String ruta)
        {
            getImagenes(ruta);
            nuevoJuego();
        }

        //
        // Metodo que revuelve los botones al azar
        //
        private void revolverBotones()
        {
            Random rnd = new Random();
            int i = 0;
            int j = 0;

            for (i = 0; i < 15; i++)
            {
                estadoBotones[i] = rnd.Next(15);
                for (j = 0; j < i; j++)
                {
                    if (estadoBotones[i] == estadoBotones[j])
                    {
                        i--;
                        break;
                    }
                }
            }
            estadoBotones[15] = 15;
        }

        //
        // Metodo que inicia un juego nuevo
        //
        private void nuevoJuego()
        {
            int i = 0;
            revolverBotones();

            movimientos = 0;
            this.toolStripStatusLabel1.Text = "Movimientos: " + movimientos.ToString();

            if (subimagenes[i] == null)
            {
                deshabilitarBotones();
                this.toolStripStatusLabel1.Text = " No hay ninguna imagen cargada";
                return;
            }

            for (i = 0; i < 16; i++)
            {
                if (estadoBotones[i] != 15)
                {
                    botones[i].BackgroundImage = subimagenes[estadoBotones[i]];
                    botones[i].Enabled = true;
                }
                else
                {
                    botones[i].BackgroundImage = null;
                    botones[i].BackColor = Color.Black;
                    botones[i].Enabled = false;
                }
            }
        }

        //
        // Metodo que deshabilita todos los botones
        //
        private void deshabilitarBotones()
        {
            for (int i = 0; i < 16; i++)
            {
                botones[i].Enabled = false;
            }
        }

        //
        // Metodo que comprueba si el vector estadoBotones esta ordenado, es decir, comprueba si todas las imagenes están ordenadas.
        //
        private bool comprobarColocado()
        {
            int i = 0;

            for (i = 0; i < 16; i++)
            {
                if (estadoBotones[i] != i)
                {
                    return false;
                }
            }

            return true;
        }

        //
        // Metodo que repinta los botones en la posicion pasada por parametro
        //
        private void repintarBoton(int i)
        {
            if (estadoBotones[i] != 15)
            {
                botones[i].BackgroundImage = subimagenes[estadoBotones[i]];
                botones[i].Enabled = true;
            }
            else
            {
                botones[i].BackColor = Color.Black;
                botones[i].BackgroundImage = null;
                botones[i].Enabled = false;
            }
        }

        //
        // Metodo que mueve una ficha
        //
        private bool moverFicha(int pos)
        {
            if (pos + 1 < 16)
            {
                if (estadoBotones[pos + 1] == 15 && ((pos + 1) % 4) != 0)
                {
                    estadoBotones[pos + 1] = estadoBotones[pos];
                    estadoBotones[pos] = 15;

                    repintarBoton(pos);
                    repintarBoton(pos + 1);

                    movimientos++;
                    return true;
                }

                if (pos + 4 < 16)
                {
                    if (estadoBotones[pos + 4] == 15)
                    {
                        estadoBotones[pos + 4] = estadoBotones[pos];
                        estadoBotones[pos] = 15;

                        repintarBoton(pos);
                        repintarBoton(pos + 4);

                        movimientos++;
                        return true;
                    }
                }
            }


            if (pos - 1 >= 0)
            {
                if (estadoBotones[pos - 1] == 15 && (pos - 1) / 4 == pos / 4)
                {
                    estadoBotones[pos - 1] = estadoBotones[pos];
                    estadoBotones[pos] = 15;

                    repintarBoton(pos);
                    repintarBoton(pos - 1);

                    movimientos++;
                    return true;
                }

                if (pos - 4 >= 0)
                {
                    if (estadoBotones[pos - 4] == 15)
                    {
                        estadoBotones[pos - 4] = estadoBotones[pos];
                        estadoBotones[pos] = 15;

                        repintarBoton(pos);
                        repintarBoton(pos - 4);

                        movimientos++;
                        return true;
                    }
                }
            }
            return false;
        }

        //
        // Metodo que redimensiona la imagen
        //
        private static Bitmap redimensionarImagen(Bitmap i, int width, int height)
        {
            // Se crea un nuevo bitmap con el tamaño final que le queremos asignar
            Bitmap imagenBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            imagenBitmap.SetResolution( Convert.ToInt32(i.HorizontalResolution), Convert.ToInt32(i.HorizontalResolution));
            
            // Se crea una zona de dibujado para el Bitmap
            Graphics imagenGraphics = Graphics.FromImage(imagenBitmap);
            
            // Se establecen los parametros para la conversion
            imagenGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            imagenGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            imagenGraphics.PixelOffsetMode =  System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Se dibuja la nueva imagen
            imagenGraphics.DrawImage( i, new Rectangle(0, 0, width, height), new Rectangle(0, 0, i.Width, i.Height), GraphicsUnit.Pixel);

            return imagenBitmap;
        }

        //
        // Metodo que obtiene y normaliza las imagenes
        //
        private Bitmap getImagen(String file)
        {
            try
            {
                Bitmap imagen = new Bitmap(file);
                imagen = redimensionarImagen(imagen, 512, 512);
                return imagen;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error!!!");
                return null;
            }
        }

        //
        // Metodo para extraer partes de imagenes a partir de coordenadas y tamaños (Similar al metodo de redimensionar)
        //
        private Bitmap getSubimage(Bitmap ori, int cordx, int cordy, int tamw, int tamh)
        {
            // Se crea un nuevo bitmap con el tamaño final que le queremos asignar
            Bitmap imagenBitmap = new Bitmap(tamw, tamh, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            imagenBitmap.SetResolution(Convert.ToInt32(ori.HorizontalResolution), Convert.ToInt32(ori.HorizontalResolution));

            // Se crea una zona de dibujado para el Bitmap
            Graphics imagenGraphics = Graphics.FromImage(imagenBitmap);

            // Se establecen los parametros para la conversion
            imagenGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            imagenGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            imagenGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // Se dibuja la nueva imagen
            imagenGraphics.DrawImage(ori, new Rectangle(0, 0, tamw, tamh), new Rectangle(cordx, cordy, tamw, tamh), GraphicsUnit.Pixel);

            return imagenBitmap;
        }

        //
        // Metodo que obtiene 15 partes a partir de una imagen más grande
        //
        private bool getImagenes(String file)
        {
            Bitmap original = getImagen(file);

            if (original == null)
            {
                return false;
            }

            // Se crean 15 segmentos con el metodo getSubimage( x, y, ancho, alto)
            subimagenes[0] = getSubimage( original,  0, 0, original.Width / 4, original.Height / 4 );
            subimagenes[1] = getSubimage( original,  (original.Width / 4) * 1, 0, original.Width / 4, original.Height / 4 );
            subimagenes[2] = getSubimage( original,  (original.Width / 4) * 2, 0, original.Width / 4, original.Height / 4 );
            subimagenes[3] = getSubimage( original,  (original.Width / 4) * 3, 0, original.Width / 4, original.Height / 4 );
            subimagenes[4] = getSubimage( original,  0, (original.Height / 4) * 1, original.Width / 4, original.Height / 4 );
            subimagenes[5] = getSubimage( original,  (original.Width / 4) * 1, (original.Height / 4) * 1, original.Width / 4, original.Height / 4 );
            subimagenes[6] = getSubimage( original,  (original.Width / 4) * 2, (original.Height / 4) * 1, original.Width / 4, original.Height / 4 );
            subimagenes[7] = getSubimage( original,  (original.Width / 4) * 3, (original.Height / 4) * 1, original.Width / 4, original.Height / 4 );
            subimagenes[8] = getSubimage( original,  0, (original.Height / 4) * 2, original.Width / 4, original.Height / 4 );
            subimagenes[9] = getSubimage( original,  (original.Width / 4) * 1, (original.Height / 4) * 2, original.Width / 4, original.Height / 4 );
            subimagenes[10] = getSubimage( original,  (original.Width / 4) * 2, (original.Height / 4) * 2, original.Width / 4, original.Height / 4 );
            subimagenes[11] = getSubimage( original,  (original.Width / 4) * 3, (original.Height / 4) * 2, original.Width / 4, original.Height / 4 );
            subimagenes[12] = getSubimage( original,  0, (original.Height / 4) * 3, original.Width / 4, original.Height / 4 );
            subimagenes[13] = getSubimage( original, (original.Width / 4) * 1, (original.Height / 4) * 3, original.Width / 4, original.Height / 4);
            subimagenes[14] = getSubimage( original, (original.Width / 4) * 2, (original.Height / 4) * 3, original.Width / 4, original.Height / 4);

            return true;
        }
    }
}
