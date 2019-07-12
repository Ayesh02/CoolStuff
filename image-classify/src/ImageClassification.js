import React, { Component } from 'react';
import * as ml5 from "ml5";

class ImageClassification extends Component {
  previewImage() {
    var file = document.querySelector("#file");
    if(file != null) {
      document.getElementById("image").src = URL.createObjectURL(file.files[0]);
    }
    var results = document.getElementById("results");
    if (results.hasChildNodes()) {
      results.childNodes.forEach(function(i) {i.remove()})
    }
  }

  classifyImage() {
    const image = document.getElementById("image");

    const classifier = ml5.imageClassifier('MobileNet', function() {
      console.log('Model Loaded!');
    });

    classifier.predict(image, function(err, outcome) {
      if(outcome != null)
      {
        console.log(outcome);
        var results = document.getElementById("results");
        results.innerText = "Results:";
        results.appendChild(document.createElement("br"));
        outcome.forEach(function(r){
          var result = document.createElement("span");
          result.innerText = " - " + r.label + " with a confidence of " + (r.confidence * 100).toFixed(4) + "%";
          results.appendChild(result);
          results.appendChild(document.createElement("br"));
        });
      }
      else {
        console.log(err);
      }
    });
  }

  render() {
    return (
      <div>
        Image Classification 101
        <div>
          <input type="file"  accept="image/*" name="image" id="file"  onChange={this.previewImage} />
          <br />
          <img src="" id="image" width="400" />
          <br />
          <button onClick={this.classifyImage}>Classify</button>
          <div id="results">

          </div>
        </div>
      </div>
    );
  }
}

export default ImageClassification;
