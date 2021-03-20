using System;
using System.ComponentModel;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Prism.Commands;
using TimerForStudying.Properties;

namespace TimerForStudying
{
    internal class TimerViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _dispatcherTimer;
        private ICommand _startCommand;
        private ICommand _addTenMinutesCommand;
        private ICommand _pauseCommand;
        private ICommand _stopCommand;
        private TimeSpan _timeSpan;
        private bool _isAddTenMinutesEnabled;
        private bool _isEditingEnbaled;
        private bool _isStartEnabled;
        private int? _minutes;
        private int? _hours;

        public TimerViewModel()
        {
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _dispatcherTimer.Tick += OnTimerTick;

            StartCommand = new DelegateCommand(OnStartCommand);
            AddTenMinutesCommand = new DelegateCommand(OnAddTenMinutes);
            PauseCommand = new DelegateCommand(OnPauseTimer);
            StopCommand = new DelegateCommand(OnStopAndReset);
            TimeSpan = new TimeSpan(0, 0, 0);

            SetDefaultProperties();

            Minutes = 0;
            Hours = 0;
        }

        public int? Minutes
        {
            get { return _minutes; }
            set
            {
                _minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public int? Hours
        {
            get { return _hours; }
            set
            {
                _hours = value;
                OnPropertyChanged(nameof(Hours));
            }
        }

        public TimeSpan TimeSpan
        {
            get { return _timeSpan; }
            set
            {
                _timeSpan = value;
                OnPropertyChanged(nameof(TimeSpan));
            }
        }

        public ICommand StopCommand
        {
            get { return _stopCommand; }
            set
            {
                _stopCommand = value;
                OnPropertyChanged(nameof(StopCommand));
            }
        }

        public ICommand PauseCommand
        {
            get { return _pauseCommand; }
            set
            {
                _pauseCommand = value;
                OnPropertyChanged(nameof(PauseCommand));
            }
        }

        public ICommand AddTenMinutesCommand
        {
            get { return _addTenMinutesCommand; }
            set
            {
                _addTenMinutesCommand = value;
                OnPropertyChanged(nameof(AddTenMinutesCommand));
            }
        }

        public ICommand StartCommand
        {
            get { return _startCommand; }
            set
            {
                _startCommand = value;
                OnPropertyChanged(nameof(StartCommand));
            }
        }

        public bool IsStartEnabled
        {
            get { return _isStartEnabled; }
            set
            {
                _isStartEnabled = value;
                OnPropertyChanged(nameof(IsStartEnabled));
            }
        }

        public bool IsAddTenMinutesEnabled
        {
            get { return _isAddTenMinutesEnabled; }
            set
            {
                _isAddTenMinutesEnabled = value;
                OnPropertyChanged(nameof(IsAddTenMinutesEnabled));
            }
        }

        public bool IsEditingEnabled
        {
            get { return _isEditingEnbaled; }
            set
            {
                _isEditingEnbaled = value;
                OnPropertyChanged(nameof(IsEditingEnabled));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnStartCommand()
        {
            if(TimeSpan == TimeSpan.Zero)
            {
                TimeSpan = new TimeSpan(Hours.Value, Minutes.Value, 0);
            }

            _dispatcherTimer.Start();
            IsStartEnabled = false;
            IsAddTenMinutesEnabled = true;
            IsEditingEnabled = false;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var isZero = TimeSpan == TimeSpan.Zero;

            if (isZero)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show(Resources.StopStudyingMessage, Resources.StopStudyingMessage, MessageBoxButton.OK, MessageBoxImage.Exclamation,
                    MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                _dispatcherTimer.Stop();

                SetDefaultProperties();
            }
            else
            {
                TimeSpan = TimeSpan.Subtract(TimeSpan.FromSeconds(1));
            }
        }

        private void OnAddTenMinutes()
        {
            TimeSpan = TimeSpan.Add(TimeSpan.FromMinutes(10));
        }

        private void OnPauseTimer()
        {
            _dispatcherTimer.Stop();
            IsStartEnabled = true;
        }

        private void OnStopAndReset()
        {
            _dispatcherTimer.Stop();
            TimeSpan = TimeSpan.Zero;

            SetDefaultProperties();

            Minutes = 0;
            Hours = 0;
        }

        private void SetDefaultProperties()
        {
            IsEditingEnabled = true;
            IsStartEnabled = true;
            IsAddTenMinutesEnabled = false;
        }
    }
}
