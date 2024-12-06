namespace TextAnalyzer  
open System.Drawing
open System.Windows.Forms

module LoadTxt =  
    let createLoadButton() =
        let loadButton = new Button(
            Text = "Load .txt File", 
            Width = 150, 
            Height = 40, 
            FlatStyle = FlatStyle.Flat, 
            BackColor = Color.FromArgb(222, 49, 99),  
            ForeColor = Color.White,  
            Font = new Font("Segoe UI", 10.0f, FontStyle.Bold)
        )
        loadButton
