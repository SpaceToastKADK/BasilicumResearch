var surveyLinkPlugin = {
  makeSurveyLink: function(linkCode){
    var a = document.createElement('a');
    var urlBase = "https://docs.google.com/forms/d/e/1FAIpQLSesiN72PB5QWJZdBCjqLGLJvRdCQwCcRuxJZphEoGXZwjXq0w/viewform?usp=pp_url&entry.1449005772=";
    var linkText = document.createTextNode("Complete Questionnaire");
    a.appendChild(linkText);
    a.href = urlBase+linkCode;
    var surveyDiv = document.getElementById("SurveyLink");
    surveyDiv.innerHTML = "";
    surveyDiv.appendChild(a);
  }
};

mergeInto(LibraryManager.library, surveyLinkPlugin);
