import * as React from "react";
import SearchBar from '../SearchBar';

declare interface ToolBarProps extends React.Props<any> {
}

declare interface ToolBarState extends React.ComponentState {}

export default class ToolBar extends React.Component<ToolBarProps, ToolBarState> {
  constructor(props) {
    super(props);

    this.handleSearchTermChanged = this.handleSearchTermChanged.bind(this);
  }

  handleSearchTermChanged(term) {
    console.log(term);
  }

  public render() {
    return (
      <div className="flex">
	<SearchBar
	  onSearch={(t) => this.handleSearchTermChanged(t)}
	/>
      </div>
    )
  }
}