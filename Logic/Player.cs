﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Logic.Annotations;

namespace Logic
{
    public class Player : IPlayer
    {
        private readonly ICellGrowthCalculator _cellGrowthCalculator;
        private readonly ISurroundingCellCalculator _surroundingCellCalculator;
        private Color _color;
        private int _playerNumber;
        private string _characterSymbol;
        private GrowthScorecard _growthScorecard = new GrowthScorecard();
        private int _totalCells;
        private int _deadCells;

        public Player(string name, Color playerCellColor, int playerNumber, string characterSymbol, 
            ICellGrowthCalculator cellGrowthCalculator, 
            ISurroundingCellCalculator surroundingCellCalculator)
        {
            Name = name;
            Color = playerCellColor;
            PlayerNumber = playerNumber;
            CharacterSymbol = characterSymbol;
            _cellGrowthCalculator = cellGrowthCalculator;
            _surroundingCellCalculator = surroundingCellCalculator;
        }

        private string _name;
        private int _mutationChancePercentage = BaseMutationChancePercentage;
        public const int BaseMutationChancePercentage = 10;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public Color Color
        {
            get => _color;
            set
            {
                if (value.Equals(_color)) return;
                _color = value;
                OnPropertyChanged();
            }
        }

        public int PlayerNumber
        {
            get => _playerNumber;
            set
            {
                if (value == _playerNumber) return;
                _playerNumber = value;
                OnPropertyChanged();
            }
        }

        public string CharacterSymbol
        {
            get => _characterSymbol;
            set
            {
                if (value == _characterSymbol) return;
                _characterSymbol = value;
                OnPropertyChanged();
            }
        }

        public GrowthScorecard GrowthScorecard
        {
            get => _growthScorecard;
            set
            {
                if (Equals(value, _growthScorecard)) return;
                _growthScorecard = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TopLeftGrowthChance));
                OnPropertyChanged(nameof(TopGrowthChance));
                OnPropertyChanged(nameof(TopRightGrowthChance));
                OnPropertyChanged(nameof(RightGrowthChance));
                OnPropertyChanged(nameof(BottomRightGrowthChance));
                OnPropertyChanged(nameof(BottomGrowthChance));
                OnPropertyChanged(nameof(BottomLeftGrowthChance));
                OnPropertyChanged(nameof(LeftGrowthChance));

            }
        }

        public int MutationChancePercentage
        {
            get => _mutationChancePercentage;
            set
            {
                if (value == _mutationChancePercentage) return;
                _mutationChancePercentage = value;
                OnPropertyChanged();
            }
        }

        public int TopLeftGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.TopLeft];
        public int TopGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.Top];
        public int TopRightGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.TopRight];
        public int RightGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.Right];
        public int BottomRightGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.BottomRight];
        public int BottomGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.Bottom];
        public int BottomLeftGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.BottomLeft];
        public int LeftGrowthChance => GrowthScorecard.GrowthChanceDictionary[RelativePosition.Left];

        public int TotalCells
        {
            get => _totalCells;
            set
            {
                if (value == _totalCells) return;
                _totalCells = value;
                OnPropertyChanged();
            }
        }

        public int DeadCells
        {
            get => _deadCells;
            set
            {
                if (value == _deadCells) return;
                _deadCells = value;
                OnPropertyChanged();
            }
        }

        public BioCell MakeCell(int cellIndex)
        {
            TotalCells++;
            return new BioCell(this, cellIndex, Color, _surroundingCellCalculator);
        }

        public CellGrowthResult CalculateCellGrowth(BioCell cell, SurroundingCells surroundingCells)
        {
            return _cellGrowthCalculator.CalculateCellGrowth(cell, this, surroundingCells);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string AddMutationChanceMessage => MutationOptionGenerator.IncreaseMutationChanceMessage;
        public string AddCornerGrowthChanceMessage => MutationOptionGenerator.IncreaseCornerGrowthChanceMessage;

        public void IncreaseMutationChance()
        {
            MutationChancePercentage += MutationOptionGenerator.AdditionalMutationPercentageChancePerAttributePoint;
            OnPropertyChanged(nameof(MutationChancePercentage));
        }

        public void IncreaseCornerGrowth()
        {
            GrowthScorecard.GrowthChanceDictionary[RelativePosition.TopLeft] +=
                MutationOptionGenerator.AdditionalCornerGrowthChancePerAttributePoint;
            GrowthScorecard.GrowthChanceDictionary[RelativePosition.TopRight] +=
                MutationOptionGenerator.AdditionalCornerGrowthChancePerAttributePoint;
            GrowthScorecard.GrowthChanceDictionary[RelativePosition.BottomRight] +=
                MutationOptionGenerator.AdditionalCornerGrowthChancePerAttributePoint;
            GrowthScorecard.GrowthChanceDictionary[RelativePosition.BottomLeft] +=
                MutationOptionGenerator.AdditionalCornerGrowthChancePerAttributePoint;

            OnPropertyChanged(nameof(TopLeftGrowthChance));
            OnPropertyChanged(nameof(TopRightGrowthChance));
            OnPropertyChanged(nameof(BottomRightGrowthChance));
            OnPropertyChanged(nameof(BottomLeftGrowthChance));
        }
    }
}