﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02_Othelo
{
    public class Board
    {
        private const char k_FirstPlayerSign = 'X';
        private const char k_SecPlayerSign = 'O';
        private const char k_EmptyCellSign = ' ';
        private int m_Size;
        private char[,] m_Matrix;
        private List<Cell> m_Optional1 = new List<Cell>();
        private List<Cell> m_Optional2 = new List<Cell>();

        public Board Clone()
        {
            Board newBoard = new Board();        
            newBoard.m_Size = m_Size;
            newBoard.m_Matrix = new char[m_Size, m_Size];
            for (int row = 0; row < m_Size; row++)
            {
                for (int col = 0; col < m_Size; col++)
                {
                    char currChar = m_Matrix[row, col];
                    newBoard.m_Matrix[row, col] = currChar;
                }
            }

            foreach (Cell option in m_Optional1)
            {
                newBoard.m_Optional1.Add(new Cell(option.X, option.Y));
            }

            foreach (Cell option in m_Optional2)
            {
                newBoard.m_Optional2.Add(new Cell(option.X, option.Y));
            }

            return newBoard;
        }

        public void Init(int i_Size)
        {
            m_Size = i_Size;
            m_Matrix = new char[m_Size, m_Size];

            for (int row = 0; row < m_Size; row++)
            {
                for (int col = 0; col < m_Size; col++)
                {
                    m_Matrix[row, col] = k_EmptyCellSign;
                }
            }

            addDefaults();
        }

        public char[,] Matrix
        {
            get { return m_Matrix; }
        }

        public int Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public List<Cell> Optionals1
        {
            get { return m_Optional1; }
        }

        public List<Cell> Optionals2
        {
            get { return m_Optional2; }
        }

        private void addDefaults()
        {
             if (m_Size == 6)
            {
                m_Matrix[2, 2] = m_Matrix[3, 3] = k_FirstPlayerSign;
                m_Matrix[2, 3] = m_Matrix[3, 2] = k_SecPlayerSign;
            }
            else
            {
                m_Matrix[3, 3] = m_Matrix[4, 4] = k_FirstPlayerSign;
                m_Matrix[3, 4] = m_Matrix[4, 3] = k_SecPlayerSign;
            }

            updateOptionals();
        }

        public bool TryUpdateMatrix(Cell i_ToUpdate, int i_CurrentPlayer)
        {
            bool isUpdateSuccess = false;
            char userSign = i_CurrentPlayer == 0 ? k_FirstPlayerSign : k_SecPlayerSign;
            if (i_CurrentPlayer == 0)
            {
                isUpdateSuccess = validateCell(m_Optional1, i_ToUpdate);
            }
            else
            {
               isUpdateSuccess = validateCell(m_Optional2, i_ToUpdate);
            }

            if (isUpdateSuccess)
            {
                update(i_ToUpdate, userSign);
            }

            return isUpdateSuccess;
        }

        private bool validateCell(List<Cell> i_OptionsArr, Cell i_ToCheck)
        {
            bool isValid = false; 
                foreach (Cell currentCell in i_OptionsArr)
                {
                    if (currentCell == i_ToCheck)
                    {
                        isValid = true;
                        break;
                    }
                }

            return isValid;
        }

        public void GetScores(out int o_Score1, out int o_Score2)
        {
            o_Score1 = 0;
            o_Score2 = 0;

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if(m_Matrix[i, j] == k_SecPlayerSign)
                    {
                        o_Score2++;
                    }
                    else if(m_Matrix[i, j] == k_FirstPlayerSign)
                    {
                        o_Score1++;
                    }
                }
            }
        }

        private void update(Cell i_ToUpdate, char i_UserSign)
        {
            updateMatrix(i_ToUpdate, i_UserSign);
            updateOptionals();
        }

        private void updateMatrix(Cell i_ToUpdate, char i_UserSign)
        {
            m_Matrix[i_ToUpdate.X, i_ToUpdate.Y] = i_UserSign;
            updateMatrixRec(i_ToUpdate, i_UserSign, 0, 1);
            updateMatrixRec(i_ToUpdate, i_UserSign, 0, -1);
            updateMatrixRec(i_ToUpdate, i_UserSign, 1, 1);
            updateMatrixRec(i_ToUpdate, i_UserSign, 1, -1);
            updateMatrixRec(i_ToUpdate, i_UserSign, -1, 1);
            updateMatrixRec(i_ToUpdate, i_UserSign, -1, -1);
            updateMatrixRec(i_ToUpdate, i_UserSign, 1, 0);
            updateMatrixRec(i_ToUpdate, i_UserSign, -1, 0);
        }

        private bool updateMatrixRec(Cell i_ToUpdate, char i_UserSign, int i_DirX, int i_DirY)
        {
            Cell nextCell = new Cell(i_ToUpdate.X + i_DirX, i_ToUpdate.Y + i_DirY);

            if (isOutOfBound(nextCell))
            {
                return false;
            }

            char currentCell = m_Matrix[i_ToUpdate.X + i_DirX, i_ToUpdate.Y + i_DirY];

            if (currentCell == i_UserSign)
            {
                return true;
            }
            else if (currentCell == k_EmptyCellSign )
            {
                return false;
            }

            bool res = updateMatrixRec(nextCell, i_UserSign, i_DirX, i_DirY);

            if (res)
            {
                m_Matrix[i_ToUpdate.X + i_DirX, i_ToUpdate.Y + i_DirY] = i_UserSign;
            }

            return res;
        }

        private bool isOutOfBound(Cell i_toCheckIfOutOfBound)
        {
            bool outOfBound = false;

            if (i_toCheckIfOutOfBound.X >= m_Size || i_toCheckIfOutOfBound.X < 0)
            {
                    outOfBound = true;
            }

            return outOfBound || (i_toCheckIfOutOfBound.Y >= m_Size || i_toCheckIfOutOfBound.Y < 0);
        }

        private void updateOptionals()
        {
            m_Optional1.Clear(); 
            m_Optional2.Clear();
            int counter = 0; 

            for (int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    if (m_Matrix[i, j] != k_EmptyCellSign)
                    {
                        updateOptionalsRec(new Cell(i + 0, j + 1), m_Matrix[i, j], 0, 1, counter);
                        updateOptionalsRec(new Cell(i + 0, j - 1), m_Matrix[i, j], 0, -1, counter);
                        updateOptionalsRec(new Cell(i + 1, j + 0), m_Matrix[i, j], 1, 0, counter);
                        updateOptionalsRec(new Cell(i + 1, j - 1), m_Matrix[i, j], 1, -1, counter);
                        updateOptionalsRec(new Cell(i + 1, j + 1), m_Matrix[i, j], 1, 1, counter);
                        updateOptionalsRec(new Cell(i + -1, j + 1), m_Matrix[i, j], -1, 1, counter);
                        updateOptionalsRec(new Cell(i - 1, j - 1), m_Matrix[i, j], -1, -1, counter);
                        updateOptionalsRec(new Cell(i - 1, j + 0), m_Matrix[i, j], -1, 0, counter);
                    }
                }
            }
        }

        private void updateOptionalsRec(Cell i_ToUpdate, char i_UserSign, int i_DirX, int i_DirY, int i_Counter)
        {
            if(isOutOfBound(i_ToUpdate))
            {
                return; 
            }

            char currentCell = m_Matrix[i_ToUpdate.X, i_ToUpdate.Y];
            if (currentCell == i_UserSign)
            {
                return;
            }
            else if (currentCell == k_EmptyCellSign)
            {
                if (i_Counter != 0)
                {
                    if (i_UserSign == k_FirstPlayerSign)
                    {
                        m_Optional1.Add(i_ToUpdate);
                    }
                    else
                    {
                        m_Optional2.Add(i_ToUpdate);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                Cell nextCell = new Cell(i_ToUpdate.X + i_DirX, i_ToUpdate.Y + i_DirY);
                updateOptionalsRec(nextCell, i_UserSign, i_DirX, i_DirY, i_Counter + 1);
            }
        }

        public bool HasOption()
        {
            return m_Optional1.Count != 0 || m_Optional2.Count != 0;
        }
    }
}
