console.log('Starting Async Basics');

setTimeout(() => {
  console.log('First Timeout');
},2000);

setTimeout(() => {
  console.log('Second Timeout');
}, 0);

console.log('Finishing up');
