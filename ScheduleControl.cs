using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PodPuppy
{
    class ScheduleControl : Control
    {
        private bool _ready = false;

        private bool[,] _data;
        private int _numCellsX, _numCellsY;
        private float _cellWidth, _cellHeight;

        private Pen _outlinePen = Pens.Black;
        private Brush _trueBrush = Brushes.PaleGreen;
        private Brush _falseBrush = Brushes.LightYellow;

        private bool _sweepValue;
        private Point? _prevCell = null;

        public ScheduleControl()
        {
            DoubleBuffered = true;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            _trueBrush = Enabled ? Brushes.PaleGreen : new SolidBrush(Color.FromArgb(208, 206, 191));
            _falseBrush = Enabled ? Brushes.LightYellow : new SolidBrush(Color.FromArgb(252, 252, 251));
            _outlinePen = Enabled ? Pens.Black : new Pen(Color.FromArgb(145, 155, 156));

            Invalidate();
        }

        public bool[,] Data
        {
            set
            {
                if (value == null)
                    return;

                _data = value;
                _numCellsX = _data.GetLength(0);
                _numCellsY = _data.GetLength(1);
                _cellWidth = (float)Width / (float)_numCellsX;
                _cellHeight = (float)Height / (float)_numCellsY;

                _ready = true;
            }
            get { return _data; }
        }

        private Point? ScreenToCell(Point screenPoint)
        {
            int x = (int)(((float)(screenPoint.X)) / _cellWidth);
            int y = (int)(((float)(screenPoint.Y)) / _cellHeight);

            if (x < 0 || x >= _numCellsX || y < 0 || y >= _numCellsY)
                return null;

            return new Point(x, y);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Enabled)
                return;

            Point? cell = ScreenToCell(e.Location);

            _prevCell = cell;

            if (cell == null)
                return;

            _sweepValue = !_data[cell.Value.X, cell.Value.Y];

            _data[cell.Value.X, cell.Value.Y] = _sweepValue;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!Enabled)
                return;

            if (e.Button == MouseButtons.Left)
            {
                Point? cell = ScreenToCell(e.Location);
                if (cell != null && cell != _prevCell)
                {
                    _prevCell = cell;
                    _data[cell.Value.X, cell.Value.Y] = _sweepValue;
                    Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_ready)
                return;

            //base.OnPaint(e);

            for (float y = 0; y < _numCellsY; y++)
            {
                for (float x = 0; x < _numCellsX; x++ )
                {
                    float left = (float)e.ClipRectangle.Left + (x * _cellWidth);
                    float top = (float)e.ClipRectangle.Top + (y * _cellHeight);

                    bool val = _data[(int)x, (int)y];
                    Brush brush = val ? _trueBrush : _falseBrush;

                    e.Graphics.FillRectangle(brush, left, top, _cellWidth, _cellHeight);
                }
            }

            e.Graphics.DrawRectangle(_outlinePen, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);

            for (float n = 0; n < _numCellsX; n++)
            {
                float penThickness = ((n % 4) == 0) ? 2f : 1f;

                Pen p = new Pen(_outlinePen.Color, penThickness);

                int x = (int)((float)e.ClipRectangle.Left + (n * _cellWidth));
                e.Graphics.DrawLine(p, x, e.ClipRectangle.Top, x, e.ClipRectangle.Bottom);
            }

            for (float n = 0; n < _numCellsY; n++)
            {
                int y = (int)((float)e.ClipRectangle.Top + (n * _cellHeight));
                e.Graphics.DrawLine(_outlinePen, e.ClipRectangle.Left, y, e.ClipRectangle.Right, y);
            }
        }
    }
}
