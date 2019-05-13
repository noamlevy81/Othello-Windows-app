﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    class UI
    {
        int m_Width;
        int m_Height;
        int m_SizeOfLogicMatrix;
        private char[,] m_MatrixPrint;

        public UI(int i_Size)
        {
            if (i_Size == 6)
            {
                m_SizeOfLogicMatrix = 6;
                m_Width = 27;
                m_Height = 14;
            }
            else
            {
                m_SizeOfLogicMatrix = 8;
                m_Width = 35;
                m_Height = 18;
            }
            m_MatrixPrint = new char[m_Height, m_Width];
        }
        public void FillUpMatrixP(char[,] i_MatrixLogic)
        {
            int counter = 0;
            int countNumber = 0;

            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    if (i %2 ==1 && j>1)
                    {
                        m_MatrixPrint[i, j] = '=';

                    }
                   else if (i == 0 && j % 4 == 0 && j!=0)
                    {
                        m_MatrixPrint[i,j] = (char)(65 + counter);
                        counter++;
                    }
                    else if(i!= 0 && i%2 ==0 && j==0)
                    {
                        m_MatrixPrint[i, j] = (char)('1'+countNumber);
                        countNumber++; 
                    }
                    else if (i%2 == 0 && j % 4 ==2 && i!= 0)
                    {
                        m_MatrixPrint[i, j] = '|';
                    }
                    else
                    {
                        m_MatrixPrint[i, j] = ' ';
                    }
                }
            }
                convertMatrixLogicToMatrixPrint(i_MatrixLogic);
                printMatirxP();
        }
        private void printMatirxP()
        {
            for (int i = 0; i < m_Height; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    Console.Write(m_MatrixPrint[i, j]);
                }
                Console.WriteLine();
            }
        }
        private void convertMatrixLogicToMatrixPrint(char[,] i_MatrixLogic)
        {
            for (int row = 0; row < m_SizeOfLogicMatrix; row++)
            {
                for(int col =0;col < m_SizeOfLogicMatrix; col++)
                {
                    if(i_MatrixLogic[row,col] == 'O')
                    {
                        m_MatrixPrint[row * 2 + 2, col * 4 + 4] = 'O';
                    }
                    else if (i_MatrixLogic[row, col] == 'X')
                    {
                        m_MatrixPrint[row * 2 + 2, col * 4 + 4] = 'X';
                    }
                }
            }
        }
    }
}
