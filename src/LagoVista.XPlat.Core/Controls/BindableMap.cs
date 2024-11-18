using LagoVista.Core.Commanding;
using LagoVista.Core.Models.Geo;
using LagoVista.IoT.DeviceManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LagoVista.XPlat.Core
{
    public class BindableMap : Map
    {
        public BindableMap()
        {
            PinsSource = new ObservableCollection<Pin>();
            PinsSource.CollectionChanged += PinsSourceOnCollectionChanged;
            base.MapClicked += BindableMap_MapClicked;
        }

        Pin _mapCenterPin = null;
        Circle _currentGeoFence = null;

        private void BindableMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            if (_mapTappedCommand != null)
            {
                _mapTappedCommand.Execute(new GeoLocation(e.Position.Latitude, e.Position.Longitude));
            }
        }

        #region PinSource Property
        public ObservableCollection<Pin> PinsSource
        {
            get { return (ObservableCollection<Pin>)GetValue(PinsSourceProperty); }
            set { SetValue(PinsSourceProperty, value); }
        }

        public static readonly BindableProperty PinsSourceProperty = BindableProperty.Create(
                                                         propertyName: "PinsSource",
                                                         returnType: typeof(ObservableCollection<Pin>),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: PinsSourcePropertyChanged);


        private static void PinsSourcePropertyChanged(BindableObject bindable, object oldvalue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            var newPinsSource = newValue as ObservableCollection<Pin>;

            if (thisInstance == null ||
                newPinsSource == null)
                return;

            UpdatePinsSource(thisInstance, newPinsSource);
        }
        private void PinsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePinsSource(this, sender as IEnumerable<Pin>);
        }

        private static void UpdatePinsSource(Map bindableMap, IEnumerable<Pin> newSource)
        {
            bindableMap.Pins.Clear();
            foreach (var pin in newSource)
                bindableMap.Pins.Add(pin);
        }
        #endregion

        #region MapSpan Property
        public MapSpan MapSpan
        {
            get { return (MapSpan)GetValue(MapSpanProperty); }
            set { SetValue(MapSpanProperty, value); }
        }

        public static readonly BindableProperty MapSpanProperty = BindableProperty.Create(
                                                         propertyName: "MapSpan",
                                                         returnType: typeof(MapSpan),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: MapSpanPropertyChanged);

        private static void MapSpanPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            var newMapSpan = newValue as MapSpan;

            thisInstance?.MoveToRegion(newMapSpan);
        }
        #endregion

        #region MapSpan Property
        public string VesselName
        {
            get { return (string)GetValue(VesselNameProperty); }
            set { SetValue(VesselNameProperty, value); }
        }

        public static readonly BindableProperty VesselNameProperty = BindableProperty.Create(
                                                         propertyName: nameof(VesselName),
                                                         returnType: typeof(string),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: "vessel",
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: VesselNamePropertyChanged);

        private static void VesselNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            if (newValue != null && thisInstance._mapCenterPin != null)
            {
                thisInstance._mapCenterPin.Label = newValue.ToString();
            }
        }
        #endregion

        #region MapCenter Property
        public GeoLocation MapCenter
        {
            get => (GeoLocation)GetValue(MapCenterProperty);
            set => SetValue(MapCenterProperty, value);
        }

        public static readonly BindableProperty MapCenterProperty = BindableProperty.Create(
                                                         propertyName: nameof(MapCenter),
                                                         returnType: typeof(GeoLocation),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: MapCenterPropertyChanged);

        public static void MapCenterPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            var newCenter = newValue as GeoLocation;
            if (newCenter != null && newCenter.HasLocation)
            {
                if (thisInstance._mapCenterPin == null)
                {
                    thisInstance._mapCenterPin = new Pin();
                    thisInstance._mapCenterPin.Position = new Position(newCenter.Latitude.Value, newCenter.Longitude.Value);
                    thisInstance._mapCenterPin.Label = thisInstance.VesselName;
                    thisInstance.Pins.Add(thisInstance._mapCenterPin);
                }
                else
                {
                    thisInstance._mapCenterPin.Position = new Position(newCenter.Latitude.Value, newCenter.Longitude.Value);
                }

                if (thisInstance.VisibleRegion == null || thisInstance.VisibleRegion.LatitudeDegrees < 0.001)
                {
                    thisInstance.MapSpan = new MapSpan(new Position(newCenter.Latitude.Value, newCenter.Longitude.Value), 0.1, 0.1);
                }
                else
                {
                    thisInstance.MapSpan = new MapSpan(new Position(newCenter.Latitude.Value, newCenter.Longitude.Value), thisInstance.VisibleRegion.LatitudeDegrees, thisInstance.VisibleRegion.LongitudeDegrees);
                }
            }
        }
        #endregion

        #region Geo Fences Property
        public ObservableCollection<GeoFence> GeoFences
        {
            get => (ObservableCollection<GeoFence>)GetValue(GeoFencesProperty);
            set => SetValue(GeoFencesProperty, value);
        }

        public static readonly BindableProperty GeoFencesProperty = BindableProperty.Create(
                                                         propertyName: nameof(GeoFences),
                                                         returnType: typeof(ObservableCollection<GeoFence>),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: GeoFencesPropertyChanged);

        public static void GeoFencesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            var oldGeoFences = newValue as ObservableCollection<GeoFence>;
            var newGeoFences = newValue as ObservableCollection<GeoFence>;
            if (oldGeoFences != null)
            {
                oldGeoFences.CollectionChanged -= thisInstance.GeoFences_CollectionChanged;
            }

            if (newGeoFences != null)
            {
                newGeoFences.CollectionChanged += thisInstance.GeoFences_CollectionChanged;
                thisInstance.RenderGeoFences();
            }
            else
            {
                thisInstance.MapElements.Clear();
            }
        }

        List<Circle> _circles = new List<Circle>();

        private void RenderGeoFences()
        {
            foreach (var circle in _circles)
            {
                MapElements.Remove(circle);
            }

            _circles.Clear();

            foreach (var fence in GeoFences)
            {
                var circle = new Circle()
                {
                    Center = new Position(fence.Center.Latitude.Value, fence.Center.Longitude.Value),
                    StrokeWidth = 1f,
                    StrokeColor = Color.LightGray,
                    FillColor = Color.FromRgba(0, 0, 1, 0.25),
                    Radius = new Distance(fence.RadiusMeters)

                };
                _circles.Add(circle);
                MapElements.Add(circle);
            };
        }

        private void GeoFences_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RenderGeoFences();
        }

        #endregion

        #region Current GeoFence Property
        public void RenderGeoFence()
        {
            if (_currentGeoFence == null)
            {
                _currentGeoFence = new Circle()
                {
                    StrokeWidth = 1f,
                    StrokeColor = Color.LightGray,
                    FillColor = Color.FromRgba(0, 0, 1, 0.25)
                };
                MapElements.Add(_currentGeoFence);
            }

            _currentGeoFence.Radius = new Distance(GeoFenceRadiusMeter);
            if (GeoFenceCenter != null)
            {
                _currentGeoFence.Center = new Position(GeoFenceCenter.Latitude.Value, GeoFenceCenter.Longitude.Value); ;
            }
        }

        public GeoLocation GeoFenceCenter
        {
            get => (GeoLocation)GetValue(GeoFenceCenterProperty);
            set => SetValue(GeoFenceCenterProperty, value);
        }

        public static readonly BindableProperty GeoFenceCenterProperty = BindableProperty.Create(
                                                         propertyName: nameof(GeoFenceCenter),
                                                         returnType: typeof(GeoLocation),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: GeoFencePropertyChanged);

        public static void GeoFencePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            thisInstance.RenderGeoFence();
        }

        public double GeoFenceRadiusMeter
        {
            get => (double)GetValue(GeoFenceRadiusMeterProperty);
            set => SetValue(GeoFenceRadiusMeterProperty, value);
        }

        public static readonly BindableProperty GeoFenceRadiusMeterProperty = BindableProperty.Create(
                                                         propertyName: nameof(GeoFenceRadiusMeter),
                                                         returnType: typeof(double),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.TwoWay,
                                                         validateValue: null,
                                                         propertyChanged: GeoFencePropertyChanged);
        #endregion  

        #region Map Tapped Command
        RelayCommand<GeoLocation> _mapTappedCommand;

        public static readonly BindableProperty MapTappedCommandProperty = BindableProperty.Create(
                                                         propertyName: nameof(MapTappedCommand),
                                                         returnType: typeof(RelayCommand<GeoLocation>),
                                                         declaringType: typeof(BindableMap),
                                                         defaultValue: null,
                                                         defaultBindingMode: BindingMode.OneWay,
                                                         validateValue: null,
                                                         propertyChanged: MapTappedCommandPropertyChanged);

        public static void MapTappedCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisInstance = bindable as BindableMap;
            thisInstance._mapTappedCommand = newValue as RelayCommand<GeoLocation>;
        }

        public RelayCommand<GeoLocation> MapTappedCommand
        {
            get => (RelayCommand<GeoLocation>)GetValue(MapTappedCommandProperty);
            set => SetValue(MapTappedCommandProperty, value);
        }
        #endregion
    }
}
