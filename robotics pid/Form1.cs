using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace robotics_pid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double
                propZ, propY,
                intZ, intY,
                dirZ, dirY,
                errY, prevErrY, errSigY = 0,
                errZ, prevErrZ, errSigZ = 0,
                setpoint = 20, pidZ, pidY, posOff, negOff, offByZ, offByY, NoiseZ = 0, NoiseY = 0;
            int
                posFlag = 0, Negflag = 0;

            Random number = new Random();

            chart1.Series.Clear();
            chart1.Series.Add("Offset Z");
            chart1.Series["Offset Z"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series.Add("Offset Y");
            chart1.Series["Offset Y"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            for (int i = 0; i < 9999; i++)
            {
                NoiseY = number.Next(0, 101);
                if (NoiseY > 50)
                {
                    NoiseY = number.Next(0, 40) / 10;
                }
                else
                {
                    NoiseY = -(number.Next(0, 40) / 10);
                }

                NoiseZ = number.Next(0, 101);
                if (NoiseZ > 50)
                {
                    NoiseZ = number.Next(0, 100) / 10;
                }
                else
                {
                    NoiseZ = -(number.Next(0, 100) / 10);
                }

                offByZ = number.Next(0, 101);
                if (posFlag == 1)
                {
                    offByZ = offByZ + 50;
                    posFlag = 0;
                }
                else if (Negflag == 1)
                {
                    offByZ = offByZ - 50;
                    Negflag = 0;
                }

                if (offByZ > 50)
                {
                    //step of 20deg per cycle
                    posOff = number.Next(0, 21);
                    posFlag = 1;
                }
                else
                {
                    offByZ = -(number.Next(0, 21));
                    Negflag = 1;
                }

                prevErrZ = offByZ;
                offByZ += NoiseZ;
                errZ = offByZ - setpoint;
                errSigZ += errZ;

                offByY = number.Next(0, 101);
                if (posFlag == 1)
                {
                    offByY = offByZ + 50;
                    posFlag = 0;
                }
                else if (Negflag == 1)
                {
                    offByY = offByZ - 50;
                    Negflag = 0;
                }

                if (offByY > 50)
                {
                    //step of 20deg per cycle
                    posOff = number.Next(0, 21);
                    posFlag = 1;
                }
                else
                {
                    offByY = -(number.Next(0, 21));
                    Negflag = 1;
                }

                prevErrY = offByY;
                offByY += NoiseY;
                errY = offByY - setpoint;
                errSigY += errY;

                propZ = 0.75 * -errZ;
                if (i == 0)
                {
                    intZ = 0.4 * (errSigZ);
                }
                else
                {
                    intZ = 0.4 * (errSigZ / i);
                }
                dirZ = 0.15 * (prevErrZ - errZ);
                pidZ = propZ + intZ + dirZ;

                propY = 0.95 * -errY;
                if (i == 0)
                {
                    intY = 0 * (errSigY);
                }
                else
                {
                    intY = 0.65 * (errSigY / i);
                }
                dirY = 0.05 * (prevErrY - errY);
                pidY = propY + intY + dirY;

                offByY += pidY;

                propZ = 0.95 * -errZ;
                if (i == 0)
                {
                    intZ = 0 * (errSigZ);
                }
                else
                {
                    intZ = 0.65 * (errSigZ / i);
                }
                dirZ = 0.05 * (prevErrZ - errZ);
                pidZ = propZ + intZ + dirZ;

                offByZ += pidZ;

                chart1.Series["Offset Z"].Points.AddXY(i, offByZ);
                chart1.Series["Offset Y"].Points.AddXY(i, offByY);
            }
        }
    }
}
