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
        private TimeSpan _timeSpan;
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
            TimeSpan = new TimeSpan(0, 0, 0);

            IsStartEnabled = true;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            TimeSpan = TimeSpan.Subtract(TimeSpan.FromSeconds(1));

            if (TimeSpan == TimeSpan.Zero)
            {
                SystemSounds.Exclamation.Play();
                MessageBox.Show(Resources.StopStudyingMessage, Resources.StopStudyingMessage, MessageBoxButton.OK, MessageBoxImage.Exclamation,
                    MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                _dispatcherTimer.Stop();
                IsStartEnabled = true;
            }
        }

        private void OnStartCommand()
        {
            TimeSpan = new TimeSpan(Hours.Value, Minutes.Value, 0);
            _dispatcherTimer.Start();
            IsStartEnabled = false;
        }
    }
}
