import * as ko from "knockout";
import {index} from "index";

describe("hello", () => {
	let i = null;
	
	beforeEach(() => {
		i = new index();	
	});
	
	it("should do shit", () => {
		expect(i.doStuff()).toBe(false);
	})
});