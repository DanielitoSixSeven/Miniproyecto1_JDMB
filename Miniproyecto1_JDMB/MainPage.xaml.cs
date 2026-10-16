using System.Globalization;

namespace Miniproyecto1_JDMB
{
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

      // Después de un operador, de "=" o de un error, empieza un número nuevo
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
        // Evitar dos puntos decimales
        if (texto == "." && Pantalla.Text.Contains("."))
          return;

        Pantalla.Text += texto;
      }
    }

    private void OnOperadorClicked(object sender, EventArgs e)
    {
      Button boton = (Button)sender;

      // Si la pantalla no tiene un número válido (por ejemplo "Error"), no hace nada
      if (!double.TryParse(Pantalla.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double valor))
        return;

      // Dos operadores seguidos: solo cambia el operador elegido
      if (operacion != "" && esperandoSegundoNumero)
      {
        operacion = boton.Text;
        return;
      }

      // Ya había una operación pendiente: se resuelve antes de continuar
      if (operacion != "")
      {
        if (!Calcular(valor, out double resultado))
        {
          MostrarError();
          return;
        }

        numeroAnterior = resultado;
        Pantalla.Text = resultado.ToString(CultureInfo.InvariantCulture);
      }
      else
      {
        numeroAnterior = valor;
      }

      operacion = boton.Text;
      esperandoSegundoNumero = true;
    }

    private void OnIgualClicked(object sender, EventArgs e)
    {
      if (operacion == "")
        return;

      if (!double.TryParse(Pantalla.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out double segundoNumero))
        return;

      if (!Calcular(segundoNumero, out double resultado))
      {
        MostrarError();
        return;
      }

      Pantalla.Text = resultado.ToString(CultureInfo.InvariantCulture);
      operacion = "";
      esperandoSegundoNumero = true;
    }

    // Hace el cálculo con numeroAnterior y la operación guardada.
    // Devuelve false si hay una división entre 0.
    private bool Calcular(double segundoNumero, out double resultado)
    {
      resultado = 0;

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
            return false;
          resultado = numeroAnterior / segundoNumero;
          break;
      }

      return true;
    }

    private void MostrarError()
    {
      Pantalla.Text = "Error";
      operacion = "";
      esperandoSegundoNumero = true;
    }

    // Botón C: reinicia todo
    private void OnBorrarTodoClicked(object sender, EventArgs e)
    {
      Pantalla.Text = "0";
      numeroAnterior = 0;
      operacion = "";
      esperandoSegundoNumero = false;
    }

    // Botón DEL: borra el último dígito
    private void OnDelClicked(object sender, EventArgs e)
    {
      // Si hay un error, DEL lo limpia
      if (Pantalla.Text == "Error")
      {
        Pantalla.Text = "0";
        return;
      }

      // No borrar un resultado ni el número mostrado justo después de un operador
      if (esperandoSegundoNumero)
        return;

      string texto = Pantalla.Text;

      // Si queda un solo dígito (o solo "-" más un dígito), vuelve a 0
      if (texto.Length <= 1 || (texto.Length == 2 && texto.StartsWith("-")))
      {
        Pantalla.Text = "0";
      }
      else
      {
        Pantalla.Text = texto.Substring(0, texto.Length - 1);
      }
    }
  }
}
