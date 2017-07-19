import * as React from "react";
import MapGL from "react-map-gl";

export interface MapProps extends React.Props<any> {
  width: number;
  height: number;
  viewport: MapGLViewport;
}

export class Map extends React.Component<MapProps, null> {
  render() {
    return (
      <MapGL
	width={this.props.width}
	height={this.props.height}
	latitude={this.props.viewport.latitude}
	longitude={this.props.viewport.longitude}
	zoom={this.props.viewport.zoom}
	mapboxApiAccessToken="pk.eyJ1IjoianVydmlzIiwiYSI6ImNqNTh5MmlnNzAya3EzMXE3eGFhMWtrczQifQ.BgT8aHg52oKkki4GV8oNpA"
      />
    )
  }
}

export default Map;