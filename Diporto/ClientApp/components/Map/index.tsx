import * as React from "react";
import MapGL, {Marker} from 'react-map-gl';

import Pin from '../PlacePin';

export interface MapProps extends React.Props<any> {
  viewport: MapGLViewport;
  places?: Place[]
}

export interface MapState extends React.ComponentState {
  viewport: MapGLViewport;
}

export class Map extends React.Component<MapProps, MapState> {
  constructor(props) {
    super(props);

    this.state = { viewport: props.viewport };
    this.updateViewport = this.updateViewport.bind(this);
  }

  componentWillReceiveProps(nextProps: MapProps) {
    const { viewport } = nextProps;
    this.setState({ viewport });
  }

  private _renderCityMarker(place: Place, index: number) {
    return (
      <Marker key={`marker-${index}`}
        longitude={place.lon}
        latitude={place.lat}>
        <Pin size={20}/>
      </Marker>
    )
  }

  updateViewport(newViewport) {
    let viewport = Object.assign({}, this.state.viewport, newViewport);

    this.setState({viewport});
  }

  render() {
    const { viewport } = this.state;

    return (
      <MapGL
        {...viewport}
        mapboxApiAccessToken="pk.eyJ1IjoianVydmlzIiwiYSI6ImNqNTh5MmlnNzAya3EzMXE3eGFhMWtrczQifQ.BgT8aHg52oKkki4GV8oNpA"
        onViewportChange={this.updateViewport}>

        {this.props.places.map(this._renderCityMarker)}
      </MapGL>
    )
  }
}

export default Map;