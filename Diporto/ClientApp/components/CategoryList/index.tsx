import * as React from 'react';

interface CategoryListProps extends React.Props<any> {
  categories?: string[]
}

interface CategoryListState extends React.ComponentState {}

export default class CategoryList extends React.Component<CategoryListProps, CategoryListState> {
  public static defaultProps: Partial<CategoryListProps> = {
    categories: []
  }

  private convertToTitleCase(snakeCaseString: string) {
    return snakeCaseString.replace('_', ' ')
  }

  public render() {
    return (
      <div>
	{this.props.categories.map(category =>
	  <span key={category} className="f7 lh-copy inline-flex bg-near-white ba b--light-gray ttu light-silver br2 ph2 ml2">{this.convertToTitleCase(category)}</span>
	)}
      </div>
    )
  }

}
