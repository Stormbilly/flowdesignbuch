using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using mystocks.data;

namespace mystocks
{
    public class MainWindow : Window
    {
        public event Action<string> SuchbegriffGeändert;
        
        public event Action<string> TitelAusgewählt;
        
        public MainWindow()
        {
            InitializeComponent();

            var throttle = new Throttle();
            var txtSuchbegriff = this.FindControl<TextBox>("txtSuchbegriff");
            txtSuchbegriff    // Workaround because TextInput event is not implemented in AvaloniaUi yet!!
                .GetObservable(TextBox.TextProperty)
                .Subscribe(text => {
                    if (string.IsNullOrWhiteSpace(text)) {
                        return;
                    }
                    throttle.ExecuteThrottled(500, () => SuchbegriffGeändert?.Invoke(text));
                });

            var cmbTitel = this.FindControl<ComboBox>("cmbTitelauswahl");
            cmbTitel.SelectionChanged += (o, e) => {
                if (cmbTitel.SelectedItem == null) {
                    return;
                }
                var symbol = ((Titel) cmbTitel.SelectedItem).Symbol;
                TitelAusgewählt?.Invoke(symbol);
            };
        }

        public void WertpapiereAktualisieren(IEnumerable<Wertpapier> wertpapiere) {
            var lstWertpapiere = this.FindControl<ListBox>("lstWertpapiere");
            var items = new List<Wertpapier>();
            items.AddRange(wertpapiere);
            lstWertpapiere.Items = items;
        }

        public void TitelAktualisieren(IEnumerable<Titel> titel) {
            var cmbTitel = this.FindControl<ComboBox>("cmbTitelauswahl");
            var items = new List<Titel>();
            items.AddRange(titel);
            cmbTitel.Items = items;
            cmbTitel.IsDropDownOpen = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}