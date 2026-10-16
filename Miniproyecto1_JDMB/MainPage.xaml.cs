namespace Miniproyecto1_JDMB
{
    using System.Globalization;

    public partial class MainPage : ContentPage
    {
        double numeroAnterior = 0;
        string operacion = "";
        bool esperandoSegundoNumero = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnNumeroClicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;
            string texto = boton.Text;


            if (esperandoSegundoNumero)
            {
                Pantalla.Text = texto == "." ? "0." : texto;
                esperandoSegundoNumero = false;
                return;
            }

            if (Pantalla.Text == "0" && texto != ".")
            {
                Pantalla.Text = texto;
            }
            else
            {
                if (texto == "." && Pantalla.Text.Contains("."))
                    return;

                Pantalla.Text += texto;
            }
        }

        private void OnOperadorClicked(object sender, EventArgs e)
        {
            Button boton = (Button)sender;

            numeroAnterior = double.Parse(Pantalla.Text, CultureInfo.InvariantCulture);
            operacion = boton.Text;
            esperandoSegundoNumero = true;
        }

        private void OnIgualClicked(object sender, EventArgs e)
        {
            if (operacion == "")
                return;

            double segundoNumero = double.Parse(Pantalla.Text, CultureInfo.InvariantCulture);
            double resultado = 0;

            switch (operacion)
            {
                case "+":
                    resultado = numeroAnterior + segundoNumero;
                    break;
                case "-":
                    resultado = numeroAnterior - segundoNumero;
                    break;
                case "×":
                    resultado = numeroAnterior * segundoNumero;
                    break;
                case "÷":
                    if (segundoNumero == 0)
                    {
                        Pantalla.Text = "Error";
                        operacion = "";
                        esperandoSegundoNumero = true;
                        return;
                    }
                    resultado = numeroAnterior / segundoNumero;
                    break;
            }

            Pantalla.Text = resultado.ToString(CultureInfo.InvariantCulture);
            operacion = "";
            esperandoSegundoNumero = true;
        }
    }
}