import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as PlacesState from '../store/Places';
import PlaceList from './PlaceList'
import ToolBar from './ToolBar'

type PlacesSearchProps =
  PlacesState.PlacesState
  & typeof PlacesState.actionCreators
  & RouteComponentProps<{ searchTerm: string }>;

class PlacesSearch extends React.Component<PlacesSearchProps, {}> {
  componentWillMount() {
    let searchTerm = this.props.match.params.searchTerm
    this.props.requestPlaces({ text: searchTerm });
  }

  componentWillReceiveProps(nextProps: PlacesSearchProps) {
    let searchTerm = nextProps.match.params.searchTerm;
    this.props.requestPlaces({ text: searchTerm });
  }

  componentWillUnmount() {
    this.props.emptySearchTerm();
  }

  public render() {
    return <div className="bg-white">
      <ToolBar/>

      { this.props.places.length === 0 ? <h2>Loading...</h2> : this.renderPlacesTable() }
    </div>
  }

  private renderPlacesTable() {
    return (
      <PlaceList
	      places={this.props.places}
      />
    )
  }
}

export default connect(
    (state: ApplicationState) => state.places, // Selects which state properties are merged into the component's props
    PlacesState.actionCreators                 // Selects which action creators are merged into the component's props
)(PlacesSearch) as typeof PlacesSearch;