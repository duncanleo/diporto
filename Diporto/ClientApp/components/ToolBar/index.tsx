import * as React from "react";
import SearchBar from '../SearchBar';

declare interface ToolBarProps extends React.Props<any> {
  searchButtonPressed?: (text: string) => void
  initialValue?: string
}

declare interface ToolBarState extends React.ComponentState {}

export default class ToolBar extends React.Component<ToolBarProps, ToolBarState> {
  constructor(props) {
    super(props);

    this.searchButtonPressed = this.searchButtonPressed.bind(this);
  }

  searchButtonPressed(term) {
    this.props.searchButtonPressed(term);
  }

  public render() {
    return (
      <div className="flex bg-light-gray pa3">
	<SearchBar
	  initialValue={this.props.initialValue}
	  onSearch={(t) => this.searchButtonPressed(t)}
	/>
      </div>
    )
  }
}