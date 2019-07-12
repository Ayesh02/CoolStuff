import React, { Component } from 'react';
import * as ml5 from "ml5";

class RNN extends Component {

  generateText(){
    function modelLoaded(){
      console.log('Model Loaded!');
    }

    console.log("generateText");
    // Create the character level generator with a pre trained model
    const rnn = ml5.LSTMGenerator('./models/woolf/');

    // Generete content
    var text = document.getElementById('textInput').value;

  }

  render(){
    return(
      <div>
        Text Prediction 101
        <br />
        <input id='textInput' type='text' />
        <br />
        <button onClick={this.generateText}>Predict</button>
      </div>
    )
  }
}

export default RNN;
