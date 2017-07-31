import * as React from 'react';
import PlaceListItem from '../PlaceListItem';
import Map from '../Map';

export interface PlaceListProps extends React.Props<any> {
  places: Place[]
}

export interface PlaceListState extends React.ComponentState {
  mapContainerDimensions: MapDimensions
}

export default class PlaceList extends React.Component<PlaceListProps, PlaceListState> {
  private mapContainer: HTMLDivElement;

  public static defaultProps: Partial<PlaceListProps> = {
    places: []
  }

  constructor(props) {
    super(props);

    this.state = {
      mapContainerDimensions: {
        height: 200,
        width: 200
      }
    }
  }

  componentDidMount() {
    const height = this.mapContainer.clientHeight;
    const width = this.mapContainer.clientWidth;
    this.setState({ mapContainerDimensions: { height, width } });
  }

  private renderPlaceList() {
    return this.props.places.map((place, index) => {
      return <PlaceListItem key={place.id} place={place} index={index}/>
    })
  }

  public render() {
    const { places } = this.props;
    const { mapContainerDimensions } = this.state;
    return (
      <div className="flex mw8 center mt4">
        <div className="w-70">
          {this.renderPlaceList()}
        </div>
        <div className="w-10"></div>
        <div className="w-30" ref={(mapContainer) => this.mapContainer = mapContainer}>
          <Map
            viewport={{latitude: places[0].lat, longitude: places[0].lon, zoom: 10, height: mapContainerDimensions.height, width: mapContainerDimensions.width}}
            places={places}
          />
        </div>
      </div>
    )
  }
}
