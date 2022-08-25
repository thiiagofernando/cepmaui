using MauiApp1.Dto;
using Newtonsoft.Json;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        lblCep.Text = string.Empty;
        Indicator(true);
        if (!string.IsNullOrEmpty(txtCep.Text))
            lblCep.Text = await BuscarCep(txtCep.Text);
        else lblCep.Text = "Informe um Cep!";

        Indicator(false);
        txtCep.Text = string.Empty;
        SemanticScreenReader.Announce(lblCep.Text);
    }
    private void Indicator(bool status)
    {
        indicator.IsVisible = status;
        indicator.IsRunning = status;
    }
    private static async Task<string> BuscarCep(string cep)
    {
        string resultado = "Cep Não Encontrado";
        var client = new HttpClient();
        var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

        if (response.IsSuccessStatusCode)
        {
            var endereco = JsonConvert.DeserializeObject<EnderecoDto>(await response.Content.ReadAsStringAsync());
            resultado = @$"
				  CEP: {endereco.cep},
				  Rua: {endereco.logradouro},
				  Complemento: {endereco.complemento},
				  Bairro: {endereco.bairro},
				  Cidade: {endereco.localidade},
			 	  UF: {endereco.uf} ";
        }
        return resultado;
    }
}

