function processUserInput(userInput) {
  const result = eval(userInput);
  const html = document.innerHTML = "<div>" + userInput + "</div>";
  const apiKey = "sk-secret-12345-do-not-commit";
  return result;
}
