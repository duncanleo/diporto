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
  constructor(props) {
    super(props)

    this.handleSearchButtonPressed = this.handleSearchButtonPressed.bind(this);
  }

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

  private handleSearchButtonPressed(text) {
    this.props.history.push(`/search/${text}`);
  }

  public render() {
    return (
      <div className="bg-white">
        <ToolBar
          initialValue={this.props.filter.text}
          searchButtonPressed={this.handleSearchButtonPressed}
        />

        { this.renderPlacesTable() }
      </div>
    )
  }

  private renderPlacesTable() {
    if (this.props.isLoading) { return <h2>Loading</h2> }

    if (this.props.places.length === 0) {
      return (
        <h2>{`No Results Found for ${this.props.filter.text}`}</h2>
            )
    } else {
      return (
        <PlaceList
          places={this.props.places}
        />
      )
    }
  }
}

export default connect(
    (state: ApplicationState) => state.places, // Selects which state properties are merged into the component's props
    PlacesState.actionCreators                 // Selects which action creators are merged into the component's props
)(PlacesSearch) as typeof PlacesSearch;