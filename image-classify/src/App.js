import React, { Component } from 'react';
import ImageClassification from './ImageClassification'
import RNN from './RNN'

class App extends Component {

  render() {
    return (
      <div>
        Machine Learning 101
        <div>
          <ImageClassification />
        </div>
        <br />
        <div>
          <RNN />
        </div>
      </div>
    );
  }
}

export default App;
