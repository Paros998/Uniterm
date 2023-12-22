using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class NSD : Form
    { 
         int i, x, y, z, r;

        public NSD()
        {
            InitializeComponent();
        }

       /* private void Form1_Load(object sender, EventArgs e)
        {

        }*/

        static void Main()
        {
            int  x, y, z, r;
            int i=0;

            Console.WriteLine("Введіть значення х");
        M2: try
            {
                x = Int32.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Значення х введено невірно!");
                goto M2;
            }
        Console.WriteLine("Введіть значення y");
    M4: try
        {
            y = Int32.Parse(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine("Значення y введено невірно!");
            goto M4;
        }
if ((x >  0) & (x <  ( (2^32) -1 ) )) 
  {
                i=0;
    if ((y >  0) & (y <  ( (2^32) -1 ) )) 
      { 
       if (x!=0) 
         {
if (y!=0) 
  {
    if (x > y)
    {
          r = x%y;
          if (r == 0)
          {
              Console.WriteLine("Найбільший спільний дільник у = {0}", y);}
              else
               { M1: z=y-1 ;
                   r=x%z;
                   if (r == 0)
                   {
                       r = y % z;
                       if (r == 0)
                       {
                           Console.WriteLine("Найбільший спільний дільник z  = ", z);
                       }
                           else goto M1;
                   }
                       else  goto M1;
              }
    }
                       else  {
                              i=0; 
                              goto M2;
                             }
  }                            
                              
                      else { i=i+1; 
                             if (i==3)
                               {
                                //M5: Console.WriteLine ("Кількість спроб вводу  досягла максимуму. Роботу завершено!");
                               }
                             else goto M4;
                            }         
}
    else
       {
           i = i + 1; 
           if (i == 3) goto M5; 
           else goto M2;
       }
                 
                   }
                     else { Console.WriteLine("Введене значення y не належить до натуральних чисел!"); 
                           i=i+1;
                           if (i == 3) goto M5;
                           else goto M4; 
                          }
               } 
                 else { Console.WriteLine("Введене значенняя х не належить до натуральних чисел!"); 
                       i=i+1; 
                       if (i == 3) goto M5; 
                       else goto M2;
                      }
M5: Console.WriteLine("Кількість спроб вводу  досягла максимуму. Роботу завершено!");
          }
    }
}



