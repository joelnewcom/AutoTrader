import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as ExceptionLogStore from '../store/ExceptionLogStore';

// At runtime, Redux will merge together...
type ExceptionLogsProps =
  ExceptionLogStore.ExceptionLogState // ... state we've requested from the Redux store
  & typeof ExceptionLogStore.actionCreators // ... plus action creators we've requested
  & RouteComponentProps; // ... plus incoming routing parameters


class ExceptionLog extends React.PureComponent<ExceptionLogsProps> {
  
    // This method is called when the component is first added to the document
    public componentDidMount() {
      this.props.requestExceptionLogs();
    }
  
    // This method is called when the route parameters change
    public componentDidUpdate() {
      this.props.requestExceptionLogs();
    }
  

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">ExceptionLog</h1>
        {this.renderExceptionsTable()}
      </React.Fragment>
    );
  }

  private renderExceptionsTable() {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>DateTime</th>
            <th>Message</th>
          </tr>
        </thead>
        <tbody>
          {this.props.exceptionLogs.map((exceptionLog: ExceptionLogStore.ExceptionLog) =>
            <tr>
              <td>{exceptionLog.dateTime}</td>
              <td>{exceptionLog.message}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.exceptionlogs, // Selects which state properties are merged into the component's props
  ExceptionLogStore.actionCreators // Selects which action creators are merged into the component's props
)(ExceptionLog as any); // eslint-disable-line @typescript-eslint/no-explicit-any
