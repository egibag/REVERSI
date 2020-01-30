using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class MainForm : Form
    {
        bool turn = true;       // True - White; False - Black;

        int turnCountTotal = 4;
        int turnBlack_Count = 0;
        int turnWhite_Count = 0;
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //DisablePanels();
            panelTurn_Color.BackColor = Color.White;

            Panel[,] cells = {
                              { P11, P12, P13,  P14,  P15, P16, P17,  P18 },
                              { P21, P22, P23,  P24,  P25, P26, P27,  P28 },
                              { P31, P32, P33,  P34,  P35, P36, P37,  P38 },
                              { P41, P42, P43,  P44,  P45, P46, P47,  P48 },
                              { P51, P52, P53,  P54,  P55, P56, P57,  P58 },
                              { P61, P62, P63,  P64,  P65, P66, P67,  P68 },
                              { P71, P72, P73,  P74,  P75, P76, P77,  P78 },
                              { P81, P82, P83,  P84,  P85, P86, P87,  P88 },
                            };

            // For Developing: show values on cells in Board
            foreach (var item in cells)
            {
                Label lab = new Label();
                lab.Text = item.Name;
                lab.Font = new Font(DefaultFont, FontStyle.Bold);
                //lab.Font = new Font(
                lab.TextAlign = ContentAlignment.BottomLeft;
                item.Controls.Add(lab);
            }

            //MessageBox.Show(Convert.ToString(cells[27].Name));

            cells[3, 3].BackColor = Color.White;
            cells[3, 3].Enabled = false;
            cells[3, 4].BackColor = Color.Black;
            cells[3, 4].Enabled = false;
            cells[4, 3].BackColor = Color.Black;
            cells[4, 3].Enabled = false;
            cells[4, 4].BackColor = Color.White;
            cells[4, 4].Enabled = false;

            //Panel[,] EvailableCellsWhite = { { P35, P36, P46, P56, } };

            cells[2, 4].BackColor = Color.Green;
            cells[2, 4].Enabled = true;
            cells[3, 5].BackColor = Color.Green;
            cells[3, 5].Enabled = true;
            cells[4, 2].BackColor = Color.Green;
            cells[4, 2].Enabled = true;
            cells[5, 3].BackColor = Color.Green;
            cells[5, 3].Enabled = true;
        }
        private void cell_click(object sender, EventArgs e)
        {
            RemoveGreenColor();

            //
            Panel tempPanel = (Panel)sender;
            if (turn)
            {
                //DisablePanels();                               

                tempPanel.BackColor = Color.White;
                panelTurn_Color.BackColor = Color.Black;
                turnBlack_Count++;

                var columNumber = (Convert.ToInt32(tempPanel.Name.TrimStart('P')) % 10);
                var rowNumber = (Convert.ToInt32(tempPanel.Name.TrimStart('P')) / 10);
                //MessageBox.Show("Masyvo reiksme: "+Convert.ToString(colorStored[rowNumber - 1, columNumber - 1]+" ROW: "+
                //   (rowNumber - 1) + " COL: " +(columNumber - 1)));
            }
            else
            {
                tempPanel.BackColor = Color.Black;
                turnWhite_Count++;
                var columNumber = (Convert.ToInt32(tempPanel.Name.TrimStart('P')) % 10);
                var rowNumber = (Convert.ToInt32(tempPanel.Name.TrimStart('P')) / 10);
                //colorStored[columNumber, rowNumber] = 2;
            }

            // CIA TURI BUTI APVERTIMO FUNKCIJA
            MainReversion(turn, tempPanel);

            if (turn)
            {
                panelTurn_Color.BackColor = Color.Black;
            }
            else
            {
                panelTurn_Color.BackColor = Color.White;
            }

            turn = !turn;
            //UnlockPosibleCells(turn);
            turnCountTotal++;

            CheckForTheWinner();
            //EnablePanels();
            //p.Enabled = false;
        }

        #region REVERTION METHODS:...
        private void MainReversion(bool turn, Panel p)
        {
            Panel tempColorPanel_1 = new Panel();
            Panel tempColorPanel_2 = new Panel();
            if (turn)
            {
                tempColorPanel_1.BackColor = Color.Black;
                tempColorPanel_2.BackColor = Color.White;
            }
            else
            {
                tempColorPanel_1.BackColor = Color.White;
                tempColorPanel_2.BackColor = Color.Black;
            }

            var columNumber = (Convert.ToInt32(p.Name.TrimStart('P')) % 10);
            var rowNumber = (Convert.ToInt32(p.Name.TrimStart('P')) / 10);

            Reversion_Left(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);
            Reversion_Right(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);

            Reversion_Top(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);
            Reversion_Top_Left(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);
            Reversion_Top_Right(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);

            Reversion_Bottom(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);
            Reversion_Bottom_Left(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);
            Reversion_Bottom_Right(columNumber, rowNumber, tempColorPanel_1, tempColorPanel_2);

            //MessageBox.Show("Masyvo reiksme: "+Convert.ToString(colorStored[rowNumber - 1, columNumber - 1]+" ROW: "+
            //   (rowNumber - 1) + " COL: " +(columNumber - 1)));

        }

        private void Reversion_Left(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (columNumber > 1)
            {
                columNumber--;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Left \n TempList.Count: " + tempList.Count);
        }
        private void Reversion_Right(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (columNumber < 7)
            {
                columNumber++;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Right \n TempList.Count: " + tempList.Count);
        }

        private void Reversion_Top(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (rowNumber > 1)
            {
                rowNumber--;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Top \n TempList.Count: " + tempList.Count);
        }
        private void Reversion_Top_Left(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (rowNumber > 1 || columNumber > 1)
            {
                rowNumber--;
                columNumber--;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)

                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Top_Left \n TempList.Count: " + tempList.Count);
        }
        private void Reversion_Top_Right(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (rowNumber > 1 || columNumber < 7)
            {
                rowNumber--;
                columNumber++;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Top_Right \n TempList.Count: " + tempList.Count);
        }

        private void Reversion_Bottom(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (rowNumber < 8)
            {
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
                rowNumber++;
            }
            //MessageBox.Show("Reversion_Bottom \n TempList.Count: " + tempList.Count);
        }
        private void Reversion_Bottom_Left(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (rowNumber < 7 || columNumber > 1)
            {
                rowNumber++;
                columNumber--;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Bottom_Left \n TempList.Count: " + tempList.Count);
        }
        private void Reversion_Bottom_Right(int columNumber, int rowNumber, Panel tempPanel_1, Panel tempPanel_2)
        {
            List<Control> tempList = new List<Control>();
            Control tempControl;
            while (rowNumber < 7 || columNumber < 7)
            {
                rowNumber++;
                columNumber++;
                string cellsName = "P" + Convert.ToString(rowNumber) + Convert.ToString(columNumber);
                tempControl = FindControl(cellsName);
                if (tempControl == null)
                {
                    break;
                }
                if (tempControl.BackColor == tempPanel_1.BackColor)
                {
                    tempList.Add(tempControl);
                    //MessageBox.Show("ROW: " + rowNumber + " COL: " + columNumber +
                    //    " " + Convert.ToString(FindControl(cellsName).BackColor));
                }
                else if (tempControl.BackColor == Color.DarkSeaGreen)
                {
                    break;
                }
                else if (tempControl.BackColor == tempPanel_2.BackColor & tempList.Count > 0)
                {
                    foreach (var item in tempList)
                    {
                        item.BackColor = tempPanel_2.BackColor;
                    }
                }
            }
            //MessageBox.Show("Reversion_Bottom_Right \n TempList.Count: " + tempList.Count);
        }
        #endregion

        //private void UnlockPosibleCells(Panel[,] cells, bool turn)

        private Control FindControl(string cellsName)
        {
            foreach (Control contr in this.Controls)
            {
                if (contr.Name == cellsName)
                {
                    return contr;
                }
            }
            return null; //////////////// !! Needs some research
        }
        private void CheckForTheWinner()
        {
            if (turnCountTotal == 15)
            {
                if (turnBlack_Count > turnWhite_Count)
                {
                    MessageBox.Show("BLACK WINS!");
                }
                else if (turnBlack_Count < turnWhite_Count)
                {
                    MessageBox.Show("BLACK WINS!");
                }
                else
                {
                    MessageBox.Show("DRAW!");
                }
            }
        }
        private void DisablePanels()
        {
            foreach (Control c in Controls)
            {
                Panel p = (Panel)c;
                p.Enabled = false;
            }
        }
        private void EnablePanels()
        {
            foreach (Control c in Controls)
            {
                Panel p = (Panel)c;
                p.Enabled = true;
            }
        }
        private void RemoveGreenColor()
        {
            foreach (Control contr in this.Controls)
            {
                if (contr.BackColor == Color.Green)
                {
                    contr.BackColor = Color.DarkSeaGreen;
                    //contr.Enabled = false;
                }
            }
        }
    }
}
